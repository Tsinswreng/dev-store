using DevStore.Bff.Checkout.Extensions;
using DevStore.WebAPI.Core.Configuration;
using DevStore.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net.Http;
using DevStore.WebAPI.Core.Extensions;

namespace DevStore.Bff.Checkout.Configuration {
	public static class ApiConfig {
		public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration) {
			services.AddControllers();

			services.Configure<AppServicesSettings>(configuration);

			services.AddCors(options => {
				options.AddPolicy("Total",
					builder =>
						builder
							.AllowAnyOrigin()
							.AllowAnyMethod()
							.AllowAnyHeader());
			});
			services.AddDefaultHealthCheck(configuration)
				.AddUrlGroup(new Uri($"{configuration["ShoppingCartUrl"]}/healthz-infra"), "Shopping Cart", tags: new[] { "infra" }, configureHttpMessageHandler: _ => HttpExtensions.ConfigureClientHandler())
				.AddUrlGroup(new Uri($"{configuration["CatalogUrl"]}/healthz-infra"), "Catalog API", tags: new[] { "infra" }, configureHttpMessageHandler: _ => HttpExtensions.ConfigureClientHandler())
				.AddUrlGroup(new Uri($"{configuration["CustomerUrl"]}/healthz-infra"), "Customer API", tags: new[] { "infra" }, configureHttpMessageHandler: _ => HttpExtensions.ConfigureClientHandler())
				.AddUrlGroup(new Uri($"{configuration["PaymentUrl"]}/healthz-infra"), "Billing API", tags: new[] { "infra" }, configureHttpMessageHandler: _ => HttpExtensions.ConfigureClientHandler())
				.AddUrlGroup(new Uri($"{configuration["OrderUrl"]}/healthz-infra"), "Order API", tags: new[] { "infra" }, configureHttpMessageHandler: _ => HttpExtensions.ConfigureClientHandler());
		}

		public static void UseApiConfiguration(this WebApplication app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			// Under certain scenarios, e.g minikube / linux environment / behind load balancer
			// https redirection could lead dev's to over complicated configuration for testing purpouses
			// In production is a good practice to keep it true
			if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true")
				app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors("Total");

			app.UseAuthConfiguration();

			app.MapControllers();

			app.UseDefaultHealthcheck();
		}
	}
}