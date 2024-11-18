namespace Sienar.Infrastructure;

/// <summary>
/// Represents a relative priority level with which an item should be treated
/// </summary>
public enum Priority
{
	/// <summary>
	/// The lowest priority level
	/// </summary>
	Lowest,

	/// <summary>
	/// A low priority level
	/// </summary>
	Low,

	/// <summary>
	/// The normal priority level
	/// </summary>
	Normal,

	/// <summary>
	/// A high priority level
	/// </summary>
	High,

	/// <summary>
	/// The highest priority level
	/// </summary>
	Highest
}