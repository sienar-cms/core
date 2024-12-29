using Microsoft.AspNetCore.Components;

namespace Sienar.Layouts;

/// <summary>
/// A standard base layout for use with Sienar
/// </summary>
public abstract class StandardLayoutBase : LayoutComponentBase
{
	/// <summary>
	/// The menu names to render with this layout
	/// </summary>
	protected string[] MenuNames = [];
}