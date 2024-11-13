using System.Threading.Tasks;

namespace Sienar.Infrastructure;

/// <summary>
/// Handles refreshing CSRF tokens
/// </summary>
public interface ICsrfTokenRefresher
{
	/// <summary>
	/// The name of the CSRF token cookie
	/// </summary>
	public const string CsrfTokenCookieName = "XSRF-TOKEN";

	/// <summary>
	/// Refreshes the CSRF token
	/// </summary>
	Task RefreshToken();
}