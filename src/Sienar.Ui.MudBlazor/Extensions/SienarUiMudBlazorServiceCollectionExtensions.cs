using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Sienar.Infrastructure;
using Sienar.Ui;

namespace Sienar.Extensions;

/// <summary>
/// Contains <see cref="IServiceCollection"/> extension methods used by the <c>Sienar.Plugin.Cms.Server</c> assembly
/// </summary>
public static class SienarUiMudBlazorServiceCollectionExtensions
{
	/// <summary>
	/// Adds the default <see cref="SienarTheme"/> for use in Sienar's <see cref="ThemeState"/>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <param name="isDarkMode">whether the theme represents dark mode or not</param>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddDefaultTheme(
		this IServiceCollection self,
		bool isDarkMode = false)
		=> AddTheme(
			self,
			new SienarTheme(),
			isDarkMode);

	/// <summary>
	/// Registers a custom <see cref="MudTheme"/> for use in Sienar's <see cref="ThemeState"/>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <param name="isDarkMode">whether the theme represents dark mode or not</param>
	/// <typeparam name="TTheme">the type of the theme to register</typeparam>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddTheme<TTheme>(
		this IServiceCollection self,
		bool isDarkMode = false)
		where TTheme : MudTheme, new()
		=> AddTheme(
			self,
			new TTheme(),
			isDarkMode);

	/// <summary>
	/// Registers a custom <see cref="MudTheme"/> for use in Sienar's <see cref="ThemeState"/>
	/// </summary>
	/// <param name="self">the service collection</param>
	/// <param name="theme">the <see cref="MudTheme"/> to use</param>
	/// <param name="isDarkMode">whether the theme represents dark mode or not</param>
	/// <returns>the service collection</returns>
	public static IServiceCollection AddTheme(
		this IServiceCollection self,
		MudTheme theme,
		bool isDarkMode = false)
	{
		var themeState = new ThemeState
		{
			Theme = theme,
			IsDarkMode = isDarkMode
		};
		self.AddSingleton(themeState);
		return self;
	}
}