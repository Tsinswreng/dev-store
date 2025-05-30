using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DevStore.Core.Communication;

namespace DevStore.Bff.Checkout.Services {
	public abstract class Service {
		protected StringContent GetContent(object dado) {
			return new StringContent(
				JsonSerializer.Serialize(dado),
				Encoding.UTF8,
				"application/json");
		}

		protected async Task<T> DeserializeResponse<T>(HttpResponseMessage ResponseMessage) {
			var options = new JsonSerializerOptions {
				PropertyNameCaseInsensitive = true
			};

			return JsonSerializer.Deserialize<T>(await ResponseMessage.Content.ReadAsStringAsync(), options);
		}

		protected bool ManageHttpResponse(HttpResponseMessage Response) {
			if (Response.StatusCode == HttpStatusCode.BadRequest) return false;

			Response.EnsureSuccessStatusCode();
			return true;
		}

		protected ResponseResult Ok() {
			return new ResponseResult();
		}
	}
}