using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Sienar.Ui;

public class List : ComponentBase
{
	[Parameter]
	public bool Ordered { get; set; }

	[Parameter]
	public required RenderFragment ChildContent { get; set; }

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		var tag = Ordered ? "ol" : "ul";

		builder.OpenElement(0, tag);
		builder.AddAttribute(1, "class", "mx-16 my-8");
		builder.AddContent(2, ChildContent);
		builder.CloseElement();
	}
}