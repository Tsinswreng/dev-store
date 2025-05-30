using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using DevStore.WebAPI.Core.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using NetDevPack.Security.JwtExtensions;

namespace DevStore.WebAPI.Core.Identity {
	public static class JwtConfig {
		public static void AddJwtConfiguration(this IServiceCollection services,
			IConfiguration configuration) {
			var appSettingsSection = configuration.GetSection("AppSettings");
			services.Configure<JwkOptions>(appSettingsSection);

			var jwkOptions = appSettingsSection.Get<JwkOptions>();
			jwkOptions.KeepFor = TimeSpan.FromMinutes(15);
			if (Debugger.IsAttached)
				IdentityModelEventSource.ShowPII = true;

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
				options.RequireHttpsMetadata = false;
				options.BackchannelHttpHandler = HttpExtensions.ConfigureClientHandler();
				options.SaveToken = true;
				options.SetJwksOptions(jwkOptions);
			});

			services.AddAuthorization();
		}

		public static void UseAuthConfiguration(this IApplicationBuilder app) {
			app.UseAuthentication();
			app.UseAuthorization();
		}
	}
}