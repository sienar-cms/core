using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sienar.Infrastructure;

namespace Sienar.Extensions;

/// <summary>
/// Contains WASM-specific extensions of the <see cref="SienarAppBuilder"/> class
/// </summary>
public static class SienarAppBuilderWasmExtensions
{
	/// <summary>
	/// Registers the default WASM application adapter
	/// </summary>
	/// <param name="self">The <see cref="SienarAppBuilder"/></param>
	/// <returns>The <see cref="SienarAppBuilder"/></returns>
	public static SienarAppBuilder AddWasmAdapter(this SienarAppBuilder self)
		=> self.AddWasmAdapter(new WasmApplicationAdapter());

	/// <summary>
	/// Registers a WASM application adapter
	/// </summary>
	/// <param name="self">The <see cref="SienarAppBuilder"/></param>
	/// <typeparam name="T">The type of the application adapter to register</typeparam>
	/// <returns>The <see cref="SienarAppBuilder"/></returns>
	public static SienarAppBuilder AddWasmAdapter<T>(this SienarAppBuilder self)
		where T : IApplicationAdapter<WebAssemblyHostBuilder>, new()
		=> self.AddWasmAdapter(new T());

	/// <summary>
	/// Registers a WASM application adapter
	/// </summary>
	/// <param name="self">The <see cref="SienarAppBuilder"/></param>
	/// <param name="adapter">The application adapter to register</param>
	/// <returns>The <see cref="SienarAppBuilder"/></returns>
	public static SienarAppBuilder AddWasmAdapter(
		this SienarAppBuilder self,
		IApplicationAdapter<WebAssemblyHostBuilder> adapter)
	{
		self.Adapter = adapter;
		return self;
	}
}
