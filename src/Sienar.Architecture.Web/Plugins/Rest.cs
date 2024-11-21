﻿using System;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sienar.Infrastructure;

namespace Sienar.Plugins;

/// <summary>
/// Configures RESTful ASP.NET architecture in a Sienar application, such as ASP.NET controllers and Swagger
/// </summary>
public class Rest : IWebPlugin
{
	/// <inheritdoc />
	public PluginData PluginData { get; } = new()
	{
		Name = "Sienar REST Plugin",
		Description = "Adds Sienar functionality as a RESTful API",
		Author = "Christian LeVesque",
		AuthorUrl = "https://levesque.dev",
		Homepage = "https://sienar.levesque.dev",
		Version = Version.Parse("0.1.0")
	};

	/// <inheritdoc />
	public void SetupDependencies(WebApplicationBuilder builder)
	{
		builder.Services
			.AddAntiforgery(o => o.HeaderName = "X-XSRF-TOKEN")
			.AddControllersWithViews(o => o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()))
			.ConfigureApiBehaviorOptions(o =>
			{
				o.InvalidModelStateResponseFactory = context =>
				{
					var details = new ValidationProblemDetails(context.ModelState)
					{
						Extensions =
						{
							["traceId"] = context.HttpContext.TraceIdentifier
						}
					};

					return new UnprocessableEntityObjectResult(details)
					{
						ContentTypes =
						{
							MediaTypeNames.Application.Json,
							MediaTypeNames.Application.Xml
						}
					};
				};
			})
			.AddJsonOptions(o =>
			{
				o.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
			});
	}

	/// <inheritdoc />
	public void SetupApp(MiddlewareProvider provider)
	{
		provider.AddWithPriority(
			Priority.High,
			app =>
			{
				if (app.Environment.IsDevelopment())
				{
					app.UseSwagger();
					app.UseSwaggerUI();
				}
			});

		provider.AddWithPriority(
			Priority.Normal,
			app => app.UseMiddleware<CsrfMiddleware>());

		provider.AddWithPriority(
			Priority.Lowest,
			app => app.MapControllers());
	}
}