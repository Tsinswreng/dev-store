using DevStore.WebApp.MVC.Extensions;
using DevStore.WebApp.MVC.Models;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevStore.WebApp.MVC.Services {
	public interface ICatalogService {
		Task<PagedViewModel<ProductViewModel>> ListAll(int pageSize, int pageIndex, string query = null);
		Task<ProductViewModel> GetById(Guid id);
	}
	public class CatalogService : Service, ICatalogService {
		private readonly HttpClient _httpClient;

		public CatalogService(HttpClient httpClient,
			IOptions<AppSettings> settings) {
			httpClient.BaseAddress = new Uri(settings.Value.CatalogUrl);

			_httpClient = httpClient;
		}

		public async Task<ProductViewModel> GetById(Guid id) {
			var Response = await _httpClient.GetAsync($"/catalog/products/{id}");

			ManageResponseErrors(Response);

			return await DeserializeResponse<ProductViewModel>(Response);
		}

		public async Task<PagedViewModel<ProductViewModel>> ListAll(int pageSize, int pageIndex, string query = null) {
			var Response = await _httpClient.GetAsync($"/catalog/products?ps={pageSize}&page={pageIndex}&q={query}");

			ManageResponseErrors(Response);

			return await DeserializeResponse<PagedViewModel<ProductViewModel>>(Response);
		}
	}
}