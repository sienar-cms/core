using Microsoft.AspNetCore.Components;

namespace Sienar.Layouts;

public abstract class SharedLayoutBase : ComponentBase
{
	[Parameter]
	public required RenderFragment ChildContent { get; set; }

	[Parameter]
	public required string[] MenuNames { get; set; }
}