using System;
using System.Net;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using DevStore.WebApp.MVC.Services;
using Polly.CircuitBreaker;
using Refit;

namespace DevStore.WebApp.MVC.Extensions {
	public class ExceptionMiddleware {
		private readonly RequestDelegate _next;
		private static IAuthService _authService;

		public ExceptionMiddleware(RequestDelegate next) {
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext, IAuthService authService) {
			_authService = authService;

			try {
				await _next(httpContext);
			} catch (CustomHttpRequestException ex) {
				await HandleRequestExceptionAsync(httpContext, ex.StatusCode);
			} catch (ValidationApiException ex) {
				await HandleRequestExceptionAsync(httpContext, ex.StatusCode);
			} catch (ApiException ex) {
				await HandleRequestExceptionAsync(httpContext, ex.StatusCode);
			} catch (BrokenCircuitException) {
				HandleCircuitBreakerExceptionAsync(httpContext);
			} catch (RpcException ex) {
				//400 Bad Request	    INTERNAL
				//401 Unauthorized      UNAUTHENTICATED
				//403 Forbidden         PERMISSION_DENIED
				//404 Not Found         UNIMPLEMENTED

				var statusCode = ex.StatusCode switch {
					StatusCode.Internal => 400,
					StatusCode.Unauthenticated => 401,
					StatusCode.PermissionDenied => 403,
					StatusCode.Unimplemented => 404,
					_ => 500
				};

				var httpStatusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), statusCode.ToString());

				await HandleRequestExceptionAsync(httpContext, httpStatusCode);
			}
		}

		private static async Task HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode) {
			if (statusCode == HttpStatusCode.Unauthorized) {
				if (_authService.ExpiredToken()) {
					var tokenRefreshed = await _authService.ValidRefreshToken();
					if (tokenRefreshed) {
						context.Response.Redirect(context.Request.Path);
						return;
					}
				}

				await _authService.Logout();
				context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
				return;
			}

			context.Response.StatusCode = (int)statusCode;
		}

		private static void HandleCircuitBreakerExceptionAsync(HttpContext context) {
			context.Response.Redirect("/system-unavailable");
		}
	}
}