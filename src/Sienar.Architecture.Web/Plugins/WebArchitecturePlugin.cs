using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Sienar.Infrastructure;

namespace Sienar.Plugins;

/// <summary>
/// Configures the Sienar app to run as a web application with auth, CORS, and other core web-based services
/// </summary>
public class WebArchitecturePlugin : IPlugin
{
	private readonly WebApplicationBuilder _builder;
	private readonly MiddlewareProvider _middlewareProvider;
	private readonly IConfigurer<AuthorizationOptions>? _authorizationConfigurer;
	private readonly IConfigurer<AuthenticationOptions>? _authenticationConfigurer;
	private readonly IConfigurer<AuthenticationBuilder>? _authenticationBuilderConfigurer;
	private readonly IConfigurer<AntiforgeryOptions>? _antiforgeryConfigurer;
	private readonly IConfigurer<CorsOptions>? _corsConfigurer;
	private readonly IConfigurer<CorsPolicyBuilder>? _corsPolicyBuilder;

	/// <summary>
	/// Creates a new instance of <c>WebArchitecturePlugin</c>
	/// </summary>
	public WebArchitecturePlugin(
		WebApplicationBuilder builder,
		MiddlewareProvider middlewareProvider,
		IConfigurer<AuthorizationOptions>? authorizationConfigurer = null,
		IConfigurer<AuthenticationOptions>? authenticationConfigurer = null,
		IConfigurer<AuthenticationBuilder>? authenticationBuilderConfigurer = null,
		IConfigurer<AntiforgeryOptions>? antiforgeryConfigurer = null,
		IConfigurer<CorsOptions>? corsConfigurer = null,
		IConfigurer<CorsPolicyBuilder>? corsPolicyBuilder = null)
	{
		_builder = builder;
		_middlewareProvider = middlewareProvider;
		_authorizationConfigurer = authorizationConfigurer;
		_authenticationConfigurer = authenticationConfigurer;
		_authenticationBuilderConfigurer = authenticationBuilderConfigurer;
		_antiforgeryConfigurer = antiforgeryConfigurer;
		_corsConfigurer = corsConfigurer;
		_corsPolicyBuilder = corsPolicyBuilder;
	}

	/// <inheritdoc />
	public void Configure()
	{
		ConfigureAuth();
		ConfigureCors();

		_middlewareProvider.AddWithPriority(
			Priority.Highest,
			app => app.UseStaticFiles());

		_middlewareProvider.AddWithNormalPriority(app => app.UseRouting());

		ConfigureAntiforgery();
	}

	private void ConfigureAuth()
	{
		if (_authenticationConfigurer is not null)
		{
			var authBuilder = _builder.Services.AddAuthentication(
				o => _authenticationConfigurer.Configure(o));
			_authenticationBuilderConfigurer?.Configure(authBuilder);

			_middlewareProvider.AddWithPriority(
				Priority.Low,
				app => app.UseAuthentication());
		}

		_middlewareProvider.AddWithPriority(
			Priority.Low,
			app => app.UseMiddleware<CsrfMiddleware>());
	
		if (_authorizationConfigurer is not null)
		{
			_builder.Services.AddAuthorization(
				o => _authorizationConfigurer.Configure(o));

			_middlewareProvider.AddWithPriority(
				Priority.Low,
				app => app.UseAuthorization());
		}
	}

	private void ConfigureAntiforgery()
	{
		if (_antiforgeryConfigurer is not null)
		{
			_builder.Services.AddAntiforgery(
				o => _antiforgeryConfigurer.Configure(o));

			_middlewareProvider.AddWithNormalPriority(
				app => app.UseAntiforgery());
		}
	}

	private void ConfigureCors()
	{
		if (_corsConfigurer is not null)
		{
			_builder.Services.AddCors(o => _corsConfigurer.Configure(o));

			_middlewareProvider.AddWithPriority(
				Priority.High,
				app =>
				{
					if (_corsPolicyBuilder is not null)
					{
						app.UseCors(o => _corsPolicyBuilder.Configure(o));
					}
					else
					{
						app.UseCors();
					}
				});
		}
	}
}
