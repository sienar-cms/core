using MudBlazor;

// ReSharper disable once CheckNamespace
namespace Sienar.Ui;

public class Radio<T> : MudRadio<T>
{
	/// <inheritdoc />
	public Radio()
	{
		Class = "d-flex";
		Color = Color.Primary;
	}
}