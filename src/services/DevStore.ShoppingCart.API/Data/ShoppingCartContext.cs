using DevStore.ShoppingCart.API.Model;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace DevStore.ShoppingCart.API.Data {
	public sealed class ShoppingCartContext : DbContext {
		public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options)
			: base(options) {
			//#見ask
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
			ChangeTracker.AutoDetectChangesEnabled = false;
		}

		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<CustomerShoppingCart> CustomerShoppingCart { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			//#modelʹ屬性中類型潙string者 -> varchar(100)
			foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
				e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
				property.SetColumnType("varchar(100)"
			);
//# CustomerShoppingCart中有ValidationResult
//#略 成員中類型潙ValidationResult者
			modelBuilder.Ignore<ValidationResult>();

			modelBuilder.Entity<CustomerShoppingCart>()
				.HasIndex(c => c.CustomerId)
				.HasName("IDX_Customer");//#Use HasDatabaseName() instead

			modelBuilder.Entity<CustomerShoppingCart>()
				.Ignore(c => c.Voucher)
				.OwnsOne(c => c.Voucher, v => {
					v.Property(vc => vc.Code)
						.HasColumnType("varchar(50)");

					v.Property(vc => vc.DiscountType);

					v.Property(vc => vc.Percentage);

					v.Property(vc => vc.Discount);
				});

			modelBuilder.Entity<CustomerShoppingCart>()
				.HasMany(c => c.Items)
				.WithOne(i => i.CustomerShoppingCart)
				.HasForeignKey(c => c.ShoppingCartId);

			foreach (var relationship in modelBuilder.Model.GetEntityTypes()
				.SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.Cascade;
		}
	}
}

/*#AI:

CREATE TABLE CustomerShoppingCart (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT NOT NULL,
    Voucher_Code VARCHAR(50),
    Voucher_DiscountType INT,
    Voucher_Percentage DECIMAL(18, 2),
    Voucher_Discount DECIMAL(18, 2),
    CONSTRAINT IDX_Customer UNIQUE (CustomerId)
);

CREATE TABLE CartItem (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ShoppingCartId INT NOT NULL,
    -- 其他 CartItem 的字段
    FOREIGN KEY (ShoppingCartId) REFERENCES CustomerShoppingCart(Id) ON DELETE CASCADE
);

-- 設置所有字符串類型的列為 varchar(100)
ALTER TABLE CustomerShoppingCart
ALTER COLUMN CustomerId VARCHAR(100);

ALTER TABLE CartItem
ALTER COLUMN ShoppingCartId VARCHAR(100);

 */