using System;
using DevStore.Bff.Checkout.Services.gRPC;
using DevStore.ShoppingCart.API.Services.gRPC;
using DevStore.WebAPI.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevStore.Bff.Checkout.Configuration {
	public static class GrpcConfig {
		public static void ConfigureGrpcServices(this IServiceCollection services, IConfiguration configuration) {
			services.AddSingleton<GrpcServiceInterceptor>();

			services.AddScoped<IShoppingCartGrpcService, ShoppingCartGrpcService>();

			services.AddGrpcClient<ShoppingCartOrders.ShoppingCartOrdersClient>(options => {
				options.Address = new Uri(configuration["ShoppingCartUrl"]);
			})
				.AddInterceptor<GrpcServiceInterceptor>()
				.AllowSelfSignedCertificate();
		}
	}
}