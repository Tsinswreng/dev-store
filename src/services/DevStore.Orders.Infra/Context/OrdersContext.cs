using System;
using System.Linq;
using System.Threading.Tasks;
using DevStore.Core.Data;
using DevStore.Core.DomainObjects;
using DevStore.Core.Mediator;
using DevStore.Core.Messages;
using DevStore.Orders.Domain.Orders;
using DevStore.Orders.Domain.Vouchers;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DevStore.Orders.Infra.Context {
	public class OrdersContext : DbContext, IUnitOfWork {
		private readonly IMediatorHandler _mediatorHandler;
		private readonly IConfiguration _configuration;

		public OrdersContext(DbContextOptions<OrdersContext> options, IMediatorHandler mediatorHandler, IConfiguration configuration)
			: base(options) {
			_mediatorHandler = mediatorHandler;
			_configuration = configuration;
		}


		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Voucher> Vouchers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
				e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
				property.SetColumnType("varchar(100)");

			modelBuilder.Ignore<Event>();
			modelBuilder.Ignore<ValidationResult>();
			modelBuilder.Entity<Order>().Property(p => p.Code).HasIdentityOptions(startValue: 1000);

			if (_configuration["AppSettings:DatabaseType"] == "Postgre") {
				modelBuilder.UseIdentityAlwaysColumns();
				modelBuilder.Entity<Order>().Property(p => p.Code).UseIdentityAlwaysColumn();
			}

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersContext).Assembly);

			foreach (var relationship in modelBuilder.Model.GetEntityTypes()
				.SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

			base.OnModelCreating(modelBuilder);
		}

		public async Task<bool> Commit() {
			foreach (var entry in ChangeTracker.Entries()
				.Where(entry => entry.Entity.GetType().GetProperty("DateAdded") != null)) {
				if (entry.State == EntityState.Added) {
					entry.Property("DateAdded").CurrentValue = DateTime.Now;
				}

				if (entry.State == EntityState.Modified) {
					entry.Property("DateAdded").IsModified = false;
				}
			}

			var sucess = await base.SaveChangesAsync() > 0;
			if (sucess) await _mediatorHandler.PublishEvents(this);

			return sucess;
		}
	}

	public static class MediatorExtension {
		public static async Task PublishEvents<T>(this IMediatorHandler mediator, T ctx) where T : DbContext {
			var entities = ctx.ChangeTracker
				.Entries<Entity>()
				.Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

			var domainEvents = entities
				.SelectMany(x => x.Entity.Notificacoes)
				.ToList();

			entities.ToList()
				.ForEach(entity => entity.Entity.ClearEvents());

			var tasks = domainEvents
				.Select(async (domainEvent) => {
					await mediator.PublishEvent(domainEvent);
				});

			await Task.WhenAll(tasks);
		}
	}
}