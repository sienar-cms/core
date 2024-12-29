using System.Threading.Tasks;
using MudBlazor;
using Sienar.Ui;

namespace Sienar.Extensions;

/// <summary>
/// Adds extensions to MudBlazor <see cref="IDialogService"/>
/// </summary>
public static class DialogServiceExtensions
{
	/// <summary>
	/// Creates a confirmation dialog
	/// </summary>
	/// <param name="self">The dialog service</param>
	/// <param name="title">The title to show in the confirmation dialog</param>
	/// <param name="question">The question to ask in the confirmation dialog</param>
	/// <param name="confirmText">The affirmative option text</param>
	/// <param name="cancelText">The negative option text</param>
	/// <param name="mainColor">The primary color of the confirmation dialog</param>
	/// <param name="cancelColor">The color of the cancel button</param>
	/// <returns>Whether the user confirmed or canceled the dialog</returns>
	public static async Task<bool> Confirm(
		this IDialogService self,
		string title,
		string question,
		string confirmText = "Yes",
		string cancelText = "No",
		Color mainColor = Color.Primary,
		Color cancelColor = Color.Secondary)
	{
		var parameters = new DialogParameters<ConfirmationDialog>
		{
			{ d => d.Title, title },
			{ d => d.Question, question },
			{ d => d.ConfirmText, confirmText },
			{ d => d.CancelText, cancelText },
			{ d => d.MainColor, mainColor },
			{ d => d.CancelColor, cancelColor }
		};

		var dialog = await self.ShowAsync<ConfirmationDialog>(string.Empty, parameters);

		var result = await dialog.Result;
		return !result.Canceled;
	}
}