using MudBlazor;

// ReSharper disable once CheckNamespace
namespace Sienar.Ui;

public class ErrorButton : MudButton
{
	public ErrorButton()
	{
		Color = Color.Error;
		ButtonType = ButtonType.Button;
		Variant = Variant.Filled;
	}
}