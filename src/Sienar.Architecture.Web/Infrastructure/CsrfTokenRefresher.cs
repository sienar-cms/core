#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace Sienar.Infrastructure;

/// <exclude />
public class CsrfTokenRefresher : ICsrfTokenRefresher
{
	private readonly IAntiforgery _antiforgery;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CsrfTokenRefresher(
		IAntiforgery antiforgery,
		IHttpContextAccessor httpContextAccessor)
	{
		_antiforgery = antiforgery;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task RefreshToken()
	{
		var tokens = _antiforgery.GetAndStoreTokens(
			_httpContextAccessor.HttpContext!);

		_httpContextAccessor.HttpContext!.Response.Cookies
			.Append(
				ICsrfTokenRefresher.CsrfTokenCookieName,
				tokens.RequestToken!,
				new CookieOptions { HttpOnly = false});
	}
}