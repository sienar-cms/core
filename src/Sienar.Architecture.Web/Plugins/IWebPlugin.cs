using Microsoft.AspNetCore.Builder;
using Sienar.Infrastructure;

namespace Sienar.Plugins;

/// <summary>
/// Represents a distributable plugin for ASP.NET web applications built with Sienar
/// </summary>
public interface IWebPlugin
{
	/// <summary>
	/// Plugin data for the current plugin
	/// </summary>
	PluginData PluginData { get; }

	/// <summary>
	/// Performs operations against the application's <see cref="WebApplicationBuilder"/>
	/// </summary>
	/// <param name="builder">the application's underlying <see cref="WebApplicationBuilder"/></param>
	void SetupDependencies(WebApplicationBuilder builder) {}

	/// <summary>
	/// Performs operations against the application's <see cref="WebApplication"/>
	/// </summary>
	/// <param name="middlewareProvider">a prioritized dictionary of middlewares</param>
	void SetupApp(MiddlewareProvider middlewareProvider) {}
}