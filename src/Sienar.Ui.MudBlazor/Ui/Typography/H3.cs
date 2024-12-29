using MudBlazor;

// ReSharper disable once CheckNamespace
namespace Sienar.Ui;

public class H3 : MudText
{
	/// <inheritdoc />
	public H3()
	{
		Typo = Typo.h3;
		Class = "mb-8";
	}
}