using MudBlazor;

// ReSharper disable once CheckNamespace
namespace Sienar.Ui;

public class Honeypot : MudTextField<string>
{
	/// <inheritdoc />
	public Honeypot()
	{
		Class = "extra-special-form-field";
		Label = "Please enter your secret key";
		Immediate = true;
		UserAttributes.Add("autocomplete", "off");
		UserAttributes.Add("tabindex", -1);
	}
}