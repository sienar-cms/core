using MudBlazor;

// ReSharper disable once CheckNamespace
namespace Sienar.Ui;

public class WarningIconButton : MudIconButton
{
	/// <inheritdoc />
	public WarningIconButton()
	{
		Color = Color.Warning;
		ButtonType = ButtonType.Button;
		Variant = Variant.Filled;
	}
}