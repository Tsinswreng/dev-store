using DevStore.Core.Communication;
using DevStore.WebApp.MVC.Extensions;
using DevStore.WebApp.MVC.Models;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevStore.WebApp.MVC.Services {
	public interface ICustomerService {
		Task<AddressViewModel> GetAddress();
		Task<ResponseResult> AddAddress(AddressViewModel address);
	}

	public class CustomerService : Service, ICustomerService {
		private readonly HttpClient _httpClient;

		public CustomerService(HttpClient httpClient, IOptions<AppSettings> settings) {
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(settings.Value.CustomerUrl);
		}

		public async Task<AddressViewModel> GetAddress() {
			var response = await _httpClient.GetAsync("/customers/address");

			if (response.StatusCode == HttpStatusCode.NotFound) return null;

			ManageResponseErrors(response);

			return await DeserializeResponse<AddressViewModel>(response);
		}

		public async Task<ResponseResult> AddAddress(AddressViewModel address) {
			var enderecoContent = GetContent(address);

			var response = await _httpClient.PostAsync("/customers/address", enderecoContent);

			if (!ManageResponseErrors(response)) return await DeserializeResponse<ResponseResult>(response);

			return ReturnOk();
		}
	}
}