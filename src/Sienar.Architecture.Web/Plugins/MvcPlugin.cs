using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sienar.Infrastructure;

namespace Sienar.Plugins;

/// <summary>
/// Configures the Sienar app to run as an MVC application with controllers, views, and Razor Pages
/// </summary>
public class MvcPlugin : IPlugin
{
	private readonly WebApplicationBuilder _builder;
	private readonly MiddlewareProvider _middlewareProvider;
	private readonly IConfigurer<MvcOptions>? _mvcConfigurer;
	private readonly IEnumerable<IConfigurer<IMvcBuilder>> _additionalMvcConfigurers;

	/// <summary>
	/// Creates a new instance of <c>MvcPlugin</c>
	/// </summary>
	public MvcPlugin(
		WebApplicationBuilder builder,
		MiddlewareProvider middlewareProvider,
		IEnumerable<IConfigurer<IMvcBuilder>> additionalMvcConfigurers, 
		IConfigurer<MvcOptions>? mvcConfigurer = null)
	{
		_builder = builder;
		_middlewareProvider = middlewareProvider;
		_mvcConfigurer = mvcConfigurer;
		_additionalMvcConfigurers = additionalMvcConfigurers;
	}

	/// <inheritdoc />
	public void Configure()
	{
		// Add MVC-adjacent services
		_builder.Services
			.AddEndpointsApiExplorer()
			.AddSwaggerGen()
			.AddScoped<ICsrfTokenRefresher, CsrfTokenRefresher>()
			.AddScoped<IReadableNotificationService, RestNotificationService>()
			.AddScoped<INotificationService>(
				sp => sp.GetRequiredService<IReadableNotificationService>())
			.AddScoped<IOperationResultMapper, OperationResultMapper>();

		// Add and configure MVC
		var mvcbuilder = _builder.Services.AddMvc(
			o => _mvcConfigurer?.Configure(o));

		foreach (var configurer in _additionalMvcConfigurers)
		{
			configurer.Configure(mvcbuilder);
		}

		_middlewareProvider.AddWithPriority(
			Priority.High,
			app =>
			{
				if (app.Environment.IsDevelopment())
				{
					app
						.UseSwagger()
						.UseSwaggerUI();
				}
			});

		_middlewareProvider.AddWithPriority(
			Priority.Lowest,
			app =>
			{
				app.MapControllers();
				app.MapRazorPages();
			});
	}

	/// <summary>
	/// Informs Sienar that this plugin depends on the <see cref="WebArchitecturePlugin"/>
	/// </summary>
	/// <param name="builder">The <see cref="SienarAppBuilder"/></param>
	[AppConfigurer]
	public static void ConfigureApp(SienarAppBuilder builder)
	{
		builder.AddPlugin<WebArchitecturePlugin>();
	}
}
