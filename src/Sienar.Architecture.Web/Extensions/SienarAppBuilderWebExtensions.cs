using Microsoft.AspNetCore.Builder;
using Sienar.Infrastructure;

namespace Sienar.Extensions;

/// <summary>
/// Contains web-specific extensions of the <see cref="SienarAppBuilder"/> class
/// </summary>
public static class SienarAppBuilderWebExtensions
{
	/// <summary>
	/// Registers the default web application adapter
	/// </summary>
	/// <param name="self">The <see cref="SienarAppBuilder"/></param>
	/// <returns>The <see cref="SienarAppBuilder"/></returns>
	public static SienarAppBuilder AddWebAdapter(this SienarAppBuilder self)
		=> self.AddWebAdapter(new WebApplicationAdapter());

	/// <summary>
	/// Registers a web application adapter
	/// </summary>
	/// <param name="self">The <see cref="SienarAppBuilder"/></param>
	/// <typeparam name="T">The type of the application adapter to register</typeparam>
	/// <returns>The <see cref="SienarAppBuilder"/></returns>
	public static SienarAppBuilder AddWebAdapter<T>(this SienarAppBuilder self)
		where T : IApplicationAdapter<WebApplicationBuilder>, new()
		=> self.AddWebAdapter(new T());

	/// <summary>
	/// Registers a web application adapter
	/// </summary>
	/// <param name="self">The <see cref="SienarAppBuilder"/></param>
	/// <param name="adapter">The application adapter</param> to register
	/// <returns>The <see cref="SienarAppBuilder"/></returns>
	public static SienarAppBuilder AddWebAdapter(
		this SienarAppBuilder self,
		IApplicationAdapter<WebApplicationBuilder> adapter)
	{
		self.Adapter = adapter;
		return self;
	}
}
