using MudBlazor;

// ReSharper disable once CheckNamespace
namespace Sienar.Ui;

public class H2 : MudText
{
	/// <inheritdoc />
	public H2()
	{
		Typo = Typo.h2;
		Class = "mb-8";
	}
}