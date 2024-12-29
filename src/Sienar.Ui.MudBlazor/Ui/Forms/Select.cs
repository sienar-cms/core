using MudBlazor;

// ReSharper disable once CheckNamespace
namespace Sienar.Ui;

public class Select<T> : MudSelect<T>
{
	/// <inheritdoc />
	public Select()
	{
		AnchorOrigin = Origin.BottomLeft;
		TransformOrigin = Origin.TopLeft;
	}
}