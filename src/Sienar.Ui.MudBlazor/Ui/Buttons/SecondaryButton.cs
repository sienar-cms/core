using MudBlazor;

// ReSharper disable once CheckNamespace
namespace Sienar.Ui;

public class SecondaryButton : MudButton
{
	public SecondaryButton()
	{
		Color = Color.Secondary;
		ButtonType = ButtonType.Button;
		Variant = Variant.Outlined;
	}
}