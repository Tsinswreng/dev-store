/*#
[2025-01-20T17:18:59.501+08:00_W4-1]
DI配置
 */
using DevStore.Customers.API.Data;
using DevStore.Customers.API.Data.Repository;
using DevStore.Customers.API.Models;
using DevStore.WebAPI.Core.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DevStore.Customers.API.Configuration {
	public static class DependencyInjectionConfig {
		public static void RegisterServices(this IServiceCollection services) {
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IAspNetUser, AspNetUser>();

			services.AddScoped<ICustomerRepository, CustomerRepository>();
			services.AddScoped<CustomerContext>();
		}
	}
}