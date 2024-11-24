using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sienar.Extensions;
using Sienar.Plugins;

namespace Sienar.Infrastructure;

/// <summary>
/// The Sienar web app builder, which is used to create Sienar applications
/// </summary>
public sealed class SienarWebAppBuilder
{
	/// <summary>
	/// The <see cref="WebApplicationBuilder"/> backing this Sienar application
	/// </summary>
	public readonly WebApplicationBuilder Builder;

	/// <summary>
	/// A dictionary that contains custom values that can be used by non-Sienar code during the application initialization process
	/// </summary>
	public readonly Dictionary<string, object> CustomItems = new();

	/// <summary>
	/// The plugin data provider added to the DI container
	/// </summary>
	public readonly IPluginDataProvider PluginDataProvider;

	/// <summary>
	/// The middlewares that should be executed at startup, arranged by priority
	/// </summary>
	public readonly MiddlewareProvider MiddlewareProvider = new();

	/// <summary>
	/// Services that are only used at application startup for configuration by Sienar
	/// </summary>
	public readonly IServiceCollection StartupServices = new ServiceCollection();

	/// <summary>
	/// The startup args as provided to <c>public static void Main(string[] args)</c>
	/// </summary>
	public string[] StartupArgs = Array.Empty<string>();

	private SienarWebAppBuilder(WebApplicationBuilder builder)
	{
		Builder = builder;
		PluginDataProvider = new PluginDataProvider();
		Builder.Services.AddSingleton(PluginDataProvider);
	}

	/// <summary>
	/// Creates a new <see cref="SienarWebAppBuilder"/> and registers core Sienar services on its service collection
	/// </summary>
	/// <param name="args">the runtime arguments supplied to <c>Program.Main()</c></param>
	/// <returns>the new <see cref="SienarWebAppBuilder"/></returns>
	public static SienarWebAppBuilder Create(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services
			.AddSienarCoreUtilities()
			.AddEndpointsApiExplorer()
			.AddSwaggerGen()
			.AddScoped<ICsrfTokenRefresher, CsrfTokenRefresher>()
			.AddScoped<IReadableNotificationService, RestNotificationService>()
			.AddScoped<INotificationService>(
				sp => sp.GetRequiredService<IReadableNotificationService>())
			.AddScoped<IOperationResultMapper, OperationResultMapper>();

		return new SienarWebAppBuilder(builder) { StartupArgs = args };
	}

	/// <summary>
	/// Adds an <see cref="IWebPlugin"/> to the Sienar app
	/// </summary>
	/// <typeparam name="TPlugin">the type of the plugin to add</typeparam>
	/// <returns>the Sienar app builder</returns>
	public SienarWebAppBuilder AddPlugin<TPlugin>()
		where TPlugin : IWebPlugin, new()
		=> AddPlugin(new TPlugin());

	/// <summary>
	/// Adds an instance of <see cref="IWebPlugin"/> to the Sienar app
	/// </summary>
	/// <param name="plugin">an instance of the plugin to add</param>
	/// <returns>the Sienar app builder</returns>
	public SienarWebAppBuilder AddPlugin(IWebPlugin plugin)
	{
		plugin.SetupStartupDependencies(StartupServices);
		plugin.SetupDependencies(Builder);
		plugin.SetupApp(MiddlewareProvider);
		PluginDataProvider.Add(plugin.PluginData);
		return this;
	}

	/// <summary>
	/// Performs operations against the application's <see cref="WebApplicationBuilder"/>
	/// </summary>
	/// <param name="configurer">an <see cref="Action{WebApplicationBuilder}"/> that accepts the <see cref="WebApplicationBuilder"/> as its only argument</param>
	/// <returns>the Sienar app builder</returns>
	public SienarWebAppBuilder SetupDependencies(Action<WebApplicationBuilder> configurer)
	{
		configurer(Builder);
		return this;
	}

	/// <summary>
	/// Used to register services with the startup DI container
	/// </summary>
	/// <param name="configurer">an <see cref="Action{IServiceCollection}"/> that accepts the <see cref="IServiceCollection"/> as its only argument</param>
	/// <returns>the Sienar app builder</returns>
	public SienarWebAppBuilder SetupStartupDependencies(Action<IServiceCollection> configurer)
	{
		configurer(StartupServices);
		return this;
	}

	/// <summary>
	/// Performs operations against the application's <see cref="WebApplication"/>
	/// </summary>
	/// <param name="configurer">an <see cref="Action{WebApplication}"/> that accepts the <see cref="WebApplication"/> as its only argument</param>
	/// <returns>the Sienar app builder</returns>
	public SienarWebAppBuilder SetupApp(Action<MiddlewareProvider> configurer)
	{
		configurer(MiddlewareProvider);
		return this;
	}

	/// <summary>
	/// Builds the final <see cref="WebApplication"/> and returns it
	/// </summary>
	/// <returns>the new <see cref="WebApplication"/></returns>
	public WebApplication Build()
	{
		StartupServices.AddSingleton(Builder.Environment);

		var container = StartupServices.BuildServiceProvider();
		using var scope = container.CreateScope();
		var startupServiceProvider = scope.ServiceProvider;

		var usesAuthorization = false;
		var usesAuthentication = false;
		var usesCors = false;

		// Configure auth
		var authorizationConfigurer = startupServiceProvider.GetService<IConfigurer<AuthorizationOptions>>();
		var authenticationConfigurer = startupServiceProvider.GetService<IConfigurer<AuthenticationOptions>>();
		var authenticationOptionsConfigurer = startupServiceProvider.GetService<IConfigurer<AuthenticationBuilder>>();

		if (authorizationConfigurer is not null)
		{
			usesAuthorization = true;
			Builder.Services.AddAuthorization(o => authorizationConfigurer.Configure(o, Builder.Configuration));
		}

		if (authenticationConfigurer is not null)
		{
			usesAuthentication = true;
			var authBuilder = Builder.Services.AddAuthentication(o => authenticationConfigurer.Configure(o, Builder.Configuration));
			authenticationOptionsConfigurer?.Configure(authBuilder, Builder.Configuration);
		}

		// Configure antiforgery
		var antiforgeryConfigurer = startupServiceProvider.GetService<IConfigurer<AntiforgeryOptions>>();
		if (antiforgeryConfigurer is not null)
		{
			Builder.Services.AddAntiforgery(
				o => antiforgeryConfigurer.Configure(
					o,
					Builder.Configuration));
		}

		// Configure MVC
		var mvcConfigurer = startupServiceProvider.GetService<IConfigurer<MvcOptions>>();
		var additionalMvcConfigurers = startupServiceProvider.GetServices<IConfigurer<IMvcBuilder>>();

		if (mvcConfigurer is not null)
		{
			var mvcBuilder = Builder.Services.AddControllersWithViews(
				o => mvcConfigurer.Configure(
					o,
					Builder.Configuration));

			foreach (var configurer in additionalMvcConfigurers)
			{
				configurer.Configure(mvcBuilder, Builder.Configuration);
			}
		}

		// Configure CORS
		var corsConfigurer = startupServiceProvider.GetService<IConfigurer<CorsOptions>>();
		if (corsConfigurer is not null)
		{
			usesCors = true;
			Builder.Services.AddCors(o => corsConfigurer.Configure(o, Builder.Configuration));
		}

		// Build the app
		var dotnetApp = Builder.Build();

		// Set up middlewares
		MiddlewareProvider.AddWithPriority(
			Priority.Highest,
			app =>
			{
				app.UseStaticFiles();
			});

		MiddlewareProvider.AddWithPriority(
			Priority.High,
			app =>
			{
				if (app.Environment.IsDevelopment())
				{
					app
						.UseSwagger()
						.UseSwaggerUI();
				}

				if (usesCors)
				{
					var corsMiddlewareConfigurer = startupServiceProvider.GetService<IConfigurer<CorsPolicyBuilder>>();
					if (corsMiddlewareConfigurer is not null)
					{
						app.UseCors(o => corsMiddlewareConfigurer.Configure(o, app.Configuration));
					}
					else
					{
						app.UseCors();
					}
				}
			});

		MiddlewareProvider.AddWithPriority(
			Priority.Normal,
			app =>
			{
				app
					.UseMiddleware<CsrfMiddleware>()
					.UseRouting();
			});

		MiddlewareProvider.AddWithPriority(
			Priority.Low,
			app =>
			{
				if (usesAuthentication)
				{
					app.UseAuthentication();
				}

				if (usesAuthorization)
				{
					app.UseAuthorization();
				}
			});

		MiddlewareProvider.AddWithPriority(
			Priority.Lowest,
			app => app.MapControllers());

		foreach (var middleware in MiddlewareProvider.AggregatePrioritized())
		{
			middleware(dotnetApp);
		}

		return dotnetApp;
	}
}