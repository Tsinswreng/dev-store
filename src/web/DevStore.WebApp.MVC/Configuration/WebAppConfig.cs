using DevStore.WebAPI.Core.Configuration;
using DevStore.WebApp.MVC.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevStore.WebApp.MVC.Configuration;

public static class WebAppConfig {
	public static void AddMvcConfiguration(this IServiceCollection services, IConfiguration configuration) {
		services.AddControllersWithViews();

		services.AddDataProtection()
				.PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"/var/data_protection_keys/"))
				.SetApplicationName("DevStoreEnterprise");

		services.Configure<ForwardedHeadersOptions>(options => {
			options.ForwardedHeaders =
				ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
		});

		services.Configure<AppSettings>(configuration);

		services.AddDefaultHealthCheck(configuration);
	}

	public static void UseMvcConfiguration(this WebApplication app) {
		app.UseForwardedHeaders();

		app.UseExceptionHandler("/error/500");
		app.UseStatusCodePagesWithRedirects("/error/{0}");

		// Under certain scenarios, e.g minikube / linux environment / behind load balancer
		// https redirection could lead dev's to over complicated configuration for testing purpouses
		// In production is a good practice to keep it true
		if (app.Configuration["USE_HTTPS_REDIRECTION"] == "true") {
			app.UseHttpsRedirection();
			app.UseHsts();
		}

		app.UseStaticFiles();

		app.UseRouting();

		app.UseIdentityConfiguration();

		app.UseMiddleware<ExceptionMiddleware>();

		app.UseDefaultHealthcheck();

		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Catalog}/{action=Index}/{id?}");
	}
}