using MudBlazor;

// ReSharper disable once CheckNamespace
namespace Sienar.Ui;

public class ErrorIconButton : MudIconButton
{
	public ErrorIconButton()
	{
		Color = Color.Error;
		ButtonType = ButtonType.Button;
		Variant = Variant.Filled;
	}
}