using System;
using DevStore.Customers.API.Configuration;
using DevStore.WebAPI.Core.Identity;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Serilog;

//Microsoft.AspNetCore.Builder:WebApplication
var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog(new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.CreateLogger());

#region Configure Services
builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration();

builder.Services.RegisterServices();

builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();
#endregion

#region Configure Pipeline

DbMigrationHelpers.EnsureSeedData(app).Wait();

app.UseSwaggerConfiguration();

app.UseApiConfiguration(app.Environment);

app.Run();

#endregion