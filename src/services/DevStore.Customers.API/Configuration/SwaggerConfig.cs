using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace DevStore.Customers.API.Configuration {
	public static class SwaggerConfig {
		public static void AddSwaggerConfiguration(this IServiceCollection services) {
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo() {//#v1是版本號
					Title = "DevStore Enterprise Customer API",
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


// 我的歉意，我的解释不够清晰。让我尝试用更简单的语言，并逐步解释这段代码的作用和目的。  我会避免使用过于专业的术语。

// 想象一下，你写了一个提供服务的程序（一个Web API）。  这个程序有很多功能，就像一个餐厅有很多菜。  要让别人知道你的程序能做什么，你需要一份菜单（API文档）。  Swagger就是帮你自动生成这份“菜单”的工具，而这段代码就是告诉程序如何使用Swagger生成这份菜单。

// 这段代码分成两部分：

// **第一部分：`AddSwaggerConfiguration` 方法**

// 这部分代码就像在餐厅后厨准备食材。  它做的事情是：

// 1. **告诉程序要使用Swagger:**  `services.AddSwaggerGen(...)`  这行代码就像告诉厨师要准备做一道菜（Swagger文档）。

// 2. **描述你的“菜” (API):** `c.SwaggerDoc("v1", ...)`  这部分就像厨师写菜谱，描述你的API是什么，叫什么名字（`Title`），能做什么（`Description`），谁做的（`Contact`），以及使用什么许可证（`License`）。  这些信息都会显示在你的“菜单”上。

// 3. **说明如何使用你的“菜” (身份验证):** `c.AddSecurityDefinition("Bearer", ...)` 和 `c.AddSecurityRequirement(...)`  这部分就像厨师说明这道菜需要什么材料（身份验证）。  这里说明你的API需要一个特殊的“通行证”（JWT token）才能访问，就像一些餐厅需要会员卡才能进入。


// **第二部分：`UseSwaggerConfiguration` 方法**

// 这部分代码就像把做好的菜端上桌。  它做的事情是：

// 1. **启用Swagger:** `app.UseSwagger();`  这行代码就像把菜端到餐桌上。

// 2. **显示Swagger UI:** `app.UseSwaggerUI(...)`  这行代码就像把菜单（Swagger UI）放在餐桌上，让顾客（开发者）可以查看你的API文档，并尝试使用你的API。  `c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");`  这行代码指定了菜单的位置。


// **总而言之:**

// 这段代码的作用是配置一个工具（Swagger），让你的程序能够自动生成并显示API文档，方便其他开发者使用你的API。  它就像一个餐厅的菜单，清晰地列出了所有菜品（API功能）及其使用方法，并且还说明了需要什么“通行证”（身份验证）才能使用这些功能。  如果你的API需要身份验证（例如，使用JWT token），这段代码也会帮你配置好身份验证相关的设置。


// 希望这次解释更容易理解。  如果你还有任何问题，请随时提出。
