using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using DevStore.WebAPI.Core.User;

namespace DevStore.Bff.Checkout.Extensions {
	public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler {
		private readonly IAspNetUser _aspNetUser;

		public HttpClientAuthorizationDelegatingHandler(IAspNetUser aspNetUser) {
			_aspNetUser = aspNetUser;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
			var authorizationHeader = _aspNetUser.GetHttpContext().Request.Headers["Authorization"];

			if (!string.IsNullOrEmpty(authorizationHeader)) {
				request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
			}

			var token = _aspNetUser.GetUserToken();

			if (token != null) {
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}

			return await base.SendAsync(request, cancellationToken);
		}
	}
}