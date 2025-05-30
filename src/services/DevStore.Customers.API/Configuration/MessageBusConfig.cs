/*#
[2025-01-20T17:21:33.317+08:00_W4-1]
消息總線
 */
using DevStore.Core.Utils;
using DevStore.Customers.API.Services;
using DevStore.MessageBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevStore.Customers.API.Configuration {
	public static class MessageBusConfig {
		public static void AddMessageBusConfiguration(this IServiceCollection services,
			IConfiguration configuration) {
			services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
				.AddHostedService<NewCustomerIntegrationHandler>();
		}
	}
}