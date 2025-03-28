using System;
using System.Net.Http;
using DevStore.Bff.Checkout.Extensions;
using Microsoft.Extensions.Options;

namespace DevStore.Bff.Checkout.Services {
	public interface IPaymentService {
	}

	public class PaymentService : Service, IPaymentService {
		private readonly HttpClient _httpClient;

		public PaymentService(HttpClient httpClient, IOptions<AppServicesSettings> settings) {
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(settings.Value.PaymentUrl);
		}
	}
}