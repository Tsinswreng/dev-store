using System;
using System.Net.Http;
using System.Threading.Tasks;
using DevStore.Bff.Checkout.Extensions;
using DevStore.Bff.Checkout.Models;
using DevStore.Core.Communication;
using Microsoft.Extensions.Options;

namespace DevStore.Bff.Checkout.Services {
	public interface IShoppingCartService {
		Task<ShoppingCartDto> GetShoppingCart();
		Task<ResponseResult> AddItem(ShoppingCartItemDto product);
		Task<ResponseResult> UpdateItem(Guid productId, ShoppingCartItemDto shoppingCart);
		Task<ResponseResult> RemoveItem(Guid productId);
		Task<ResponseResult> ApplyVoucher(VoucherDTO voucher);
	}

	public class ShoppingCartService : Service, IShoppingCartService {
		private readonly HttpClient _httpClient;

		public ShoppingCartService(HttpClient httpClient, IOptions<AppServicesSettings> settings) {
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(settings.Value.ShoppingCartUrl);
		}

		public async Task<ShoppingCartDto> GetShoppingCart() {
			var response = await _httpClient.GetAsync("/shopping-cart");

			ManageHttpResponse(response);

			return await DeserializeResponse<ShoppingCartDto>(response);
		}

		public async Task<ResponseResult> AddItem(ShoppingCartItemDto product) {
			var itemContent = GetContent(product);

			var response = await _httpClient.PostAsync("/shopping-cart", itemContent);

			if (!ManageHttpResponse(response)) return await DeserializeResponse<ResponseResult>(response);

			return Ok();
		}

		public async Task<ResponseResult> UpdateItem(Guid productId, ShoppingCartItemDto shoppingCart) {
			var itemContent = GetContent(shoppingCart);

			var Response = await _httpClient.PutAsync($"/shopping-cart/{shoppingCart.ProductId}", itemContent);

			if (!ManageHttpResponse(Response)) return await DeserializeResponse<ResponseResult>(Response);

			return Ok();
		}

		public async Task<ResponseResult> RemoveItem(Guid productId) {
			var Response = await _httpClient.DeleteAsync($"/shopping-cart/{productId}");

			if (!ManageHttpResponse(Response)) return await DeserializeResponse<ResponseResult>(Response);

			return Ok();
		}

		public async Task<ResponseResult> ApplyVoucher(VoucherDTO voucher) {
			var itemContent = GetContent(voucher);

			var Response = await _httpClient.PostAsync("/shopping-cart/apply-voucher/", itemContent);

			if (!ManageHttpResponse(Response)) return await DeserializeResponse<ResponseResult>(Response);

			return Ok();
		}
	}
}