using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DevStore.WebApp.MVC.Configuration {
	public static class IdentityConfig {
		public static void AddIdentityConfiguration(this IServiceCollection services) {
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options => {
					options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
					options.LoginPath = "/login";
					options.AccessDeniedPath = "/erro/403";
				});

			services.AddHttpContextAccessor();

		}

		public static void UseIdentityConfiguration(this IApplicationBuilder app) {
			app.UseAuthentication();
			app.UseAuthorization();
		}
	}
}