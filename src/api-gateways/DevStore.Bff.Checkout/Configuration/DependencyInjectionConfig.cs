using System;
using DevStore.Bff.Checkout.Extensions;
using DevStore.Bff.Checkout.Services;
using DevStore.WebAPI.Core.Extensions;
using DevStore.WebAPI.Core.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace DevStore.Bff.Checkout.Configuration {
	public static class DependencyInjectionConfig {
		public static void RegisterServices(this IServiceCollection services) {
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IAspNetUser, AspNetUser>();

			services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

			services.AddHttpClient<ICatalogService, CatalogService>()
				.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
				.AllowSelfSignedCertificate()
				.AddPolicyHandler(PollyExtensions.EsperarTentar())
				.AddTransientHttpErrorPolicy(
					p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

			services.AddHttpClient<IShoppingCartService, ShoppingCartService>()
				.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
				.AllowSelfSignedCertificate()
				.AddPolicyHandler(PollyExtensions.EsperarTentar())
				.AddTransientHttpErrorPolicy(
					p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

			services.AddHttpClient<IOrderService, OrderService>()
				.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
				.AllowSelfSignedCertificate()
				.AddPolicyHandler(PollyExtensions.EsperarTentar())
				.AddTransientHttpErrorPolicy(
					p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

			services.AddHttpClient<ICustomerService, CustomerService>()
				.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
				.AllowSelfSignedCertificate()
				.AddPolicyHandler(PollyExtensions.EsperarTentar())
				.AddTransientHttpErrorPolicy(
					p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
		}
	}
}