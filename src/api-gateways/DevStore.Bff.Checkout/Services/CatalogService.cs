using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DevStore.Bff.Checkout.Extensions;
using DevStore.Bff.Checkout.Models;
using Microsoft.Extensions.Options;

namespace DevStore.Bff.Checkout.Services {
	public interface ICatalogService {
		Task<ProductDto> GetById(Guid id);
		Task<IEnumerable<ProductDto>> GetItems(IEnumerable<Guid> ids);
	}

	public class CatalogService : Service, ICatalogService {
		private readonly HttpClient _httpClient;

		public CatalogService(HttpClient httpClient, IOptions<AppServicesSettings> settings) {
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(settings.Value.CatalogUrl);
		}

		public async Task<ProductDto> GetById(Guid id) {
			var Response = await _httpClient.GetAsync($"/catalog/products/{id}");

			ManageHttpResponse(Response);

			return await DeserializeResponse<ProductDto>(Response);
		}

		public async Task<IEnumerable<ProductDto>> GetItems(IEnumerable<Guid> ids) {
			var idsRequest = string.Join(",", ids);

			var Response = await _httpClient.GetAsync($"/catalog/products/list/{idsRequest}/");

			ManageHttpResponse(Response);

			return await DeserializeResponse<IEnumerable<ProductDto>>(Response);
		}
	}
}