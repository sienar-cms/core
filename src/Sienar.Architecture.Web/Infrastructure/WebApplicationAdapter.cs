using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sienar.Plugins;

namespace Sienar.Infrastructure;

/// <summary>
/// Maps Sienar application method calls to underlying <see cref="WebApplicationBuilder"/> method calls
/// </summary>
public class WebApplicationAdapter : IApplicationAdapter<WebApplicationBuilder>
{
	/// <inheritdoc />
	public WebApplicationBuilder Builder { get; private set; } = null!;

	/// <inheritdoc />
	public void Create(
		string[] args,
		IServiceCollection startupServices)
	{
		Builder = WebApplication.CreateBuilder(args);

		startupServices
			.AddSingleton(Builder)
			.AddSingleton(Builder.Environment)
			.AddSingleton<IConfiguration>(Builder.Configuration)
			.AddSingleton<IApplicationAdapter>(this)
			.AddSingleton<MiddlewareProvider>();
	}

	/// <inheritdoc />
	public object Build(IServiceProvider sp)
	{
		Builder.Services
			.AddSingleton(sp.GetRequiredService<IDashboardProvider>())
			.AddSingleton(sp.GetRequiredService<IMenuProvider>())
			.AddSingleton(sp.GetRequiredService<IPluginDataProvider>())
			.AddSingleton(sp.GetRequiredService<IScriptProvider>())
			.AddSingleton(sp.GetRequiredService<IStyleProvider>());

		var app = Builder.Build();

		var middlewareProvider = sp.GetRequiredService<MiddlewareProvider>();

		foreach (var middleware in middlewareProvider.AggregatePrioritized())
		{
			middleware(app);
		}

		return app;
	}

	/// <inheritdoc />
	public void AddServices(Action<IServiceCollection> configurer)
	{
		configurer(Builder.Services);
	}
}
