using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace DevStore.Bff.Checkout.Configuration {
	public static class SwaggerConfig {
		public static void AddSwaggerConfiguration(this IServiceCollection services) {
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo() {
					Title = "DevStore Enterprise Checkout BFF - API Gateway",
					Description = "This API is part of online course ASP.NET Core Enterprise Applications.",
					Contact = new OpenApiContact() { Name = "Eduardo Pires", Email = "contato@desenvolvedor.io" },
					License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/Licenses/MIT") }
				});

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
					Description = "Add you JWT token: Bearer {token}",
					Name = "Authorization",
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey
				});


				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new string[] {}
					}
				});

			});
		}

		public static void UseSwaggerConfiguration(this IApplicationBuilder app) {
			app.UseSwagger();
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
			});
		}
	}
}