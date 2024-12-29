using MudBlazor;

// ReSharper disable once CheckNamespace
namespace Sienar.Ui;

public class Textbox<T> : MudTextField<T>
{
	/// <inheritdoc />
	public Textbox()
	{
		Variant = Variant.Text;
		Immediate = true;
	}
}