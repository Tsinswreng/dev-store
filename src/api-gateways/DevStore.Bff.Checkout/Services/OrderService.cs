using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DevStore.Bff.Checkout.Extensions;
using DevStore.Bff.Checkout.Models;
using DevStore.Core.Communication;
using Microsoft.Extensions.Options;

namespace DevStore.Bff.Checkout.Services {
	public interface IOrderService {
		Task<ResponseResult> FinishOrder(OrderDto order);
		Task<OrderDto> GetLastOrder();
		Task<IEnumerable<OrderDto>> GetCustomers();

		Task<VoucherDTO> GetVoucherByCode(string code);
	}

	public class OrderService : Service, IOrderService {
		private readonly HttpClient _httpClient;

		public OrderService(HttpClient httpClient, IOptions<AppServicesSettings> settings) {
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(settings.Value.OrderUrl);
		}

		public async Task<ResponseResult> FinishOrder(OrderDto order) {
			var orderContent = GetContent(order);

			var response = await _httpClient.PostAsync("/orders", orderContent);

			if (!ManageHttpResponse(response)) return await DeserializeResponse<ResponseResult>(response);

			return Ok();
		}

		public async Task<OrderDto> GetLastOrder() {
			var response = await _httpClient.GetAsync("/orders/last");

			if (response.StatusCode == HttpStatusCode.NoContent) return null;

			ManageHttpResponse(response);

			return await DeserializeResponse<OrderDto>(response);
		}

		public async Task<IEnumerable<OrderDto>> GetCustomers() {
			var response = await _httpClient.GetAsync("/orders/customers");

			if (response.StatusCode == HttpStatusCode.NoContent) return null;

			ManageHttpResponse(response);

			return await DeserializeResponse<IEnumerable<OrderDto>>(response);
		}

		public async Task<VoucherDTO> GetVoucherByCode(string code) {
			var response = await _httpClient.GetAsync($"/voucher/{code}");

			if (response.StatusCode == HttpStatusCode.NoContent) return null;

			ManageHttpResponse(response);

			return await DeserializeResponse<VoucherDTO>(response);
		}
	}
}