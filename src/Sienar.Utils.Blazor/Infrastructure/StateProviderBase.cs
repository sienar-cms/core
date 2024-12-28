using System;

namespace Sienar.Infrastructure;

/// <summary>
/// A base state provider with a public <c>OnChange</c> event
/// </summary>
public abstract class StateProviderBase
{
	/// <summary>
	/// The event that fires when a state provider's state has changed
	/// </summary>
	public event Action? OnChange;

	/// <summary>
	/// Notifies listeners that a state provider's state has changed
	/// </summary>
	protected void NotifyStateChanged() => OnChange?.Invoke();
}
