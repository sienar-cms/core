using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sienar.Infrastructure;

/// <summary>
/// Supplies automatically-refreshing CSRF protection to Sienar applications
/// </summary>
public class CsrfMiddleware
{
	private readonly RequestDelegate _next;

	/// <exclude />
	public CsrfMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	/// <exclude />
	public async Task InvokeAsync(
		HttpContext context,
		ICsrfTokenRefresher tokenRefresher)
	{
		// if the request doesn't contain the CSRF token cookie
		// generate the antiforgery token
		if (!context.Request.Cookies.ContainsKey(ICsrfTokenRefresher.CsrfTokenCookieName))
		{
			await tokenRefresher.RefreshToken();
		}

		await _next(context);
	}
}