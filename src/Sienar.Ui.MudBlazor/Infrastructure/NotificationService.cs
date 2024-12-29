using MudBlazor;

namespace Sienar.Infrastructure;

public class NotificationService : INotificationService
{
	private readonly ISnackbar _snackbar;

	public NotificationService(ISnackbar snackbar)
	{
		_snackbar = snackbar;
	}

	/// <inheritdoc />
	public void Success(string message)
	{
		_snackbar.Add(message, Severity.Success, ConfigureSnackbar);
	}

	/// <inheritdoc />
	public void Warning(string message)
	{
		_snackbar.Add(message, Severity.Warning, ConfigureDangerSnackbar);
	}

	/// <inheritdoc />
	public void Info(string message)
	{
		_snackbar.Add(message, Severity.Info, ConfigureSnackbar);
	}

	/// <inheritdoc />
	public void Error(string message)
	{
		_snackbar.Add(message, Severity.Error, ConfigureDangerSnackbar);
	}

	private void ConfigureSnackbar(SnackbarOptions o)
	{
		o.ShowTransitionDuration = 500;
		o.HideTransitionDuration = 500;
		o.VisibleStateDuration = 5000;
	}

	private void ConfigureDangerSnackbar(SnackbarOptions o)
	{
		o.ShowTransitionDuration = 500;
		o.HideTransitionDuration = 500;
		o.RequireInteraction = true;
	}
}