﻿using System.Collections.Generic;

namespace Sienar.Infrastructure;

/// <summary>
/// Arranges multiple lists by <see cref="Priority"/>
/// </summary>
/// <typeparam name="T">the type of the item to be contained in a <see cref="List{T}"/></typeparam>
public class PrioritizedListDictionary<T> : Dictionary<Priority, List<T>>
{
	/// <summary>
	/// Adds items with the specified priority level
	/// </summary>
	/// <param name="priority">The priority at which to add items</param>
	/// <param name="prioritizedItems">The items to add</param>
	/// <returns>self</returns>
	public PrioritizedListDictionary<T> AddWithPriority(
		Priority priority,
		params T[] prioritizedItems)
	{
		if (!TryGetValue(priority, out var items))
		{
			items = [];
			this[priority] = items;
		}

		items.AddRange(prioritizedItems);

		return this;
	}

	/// <summary>
	/// Adds items with a normal priority level
	/// </summary>
	/// <param name="prioritizedItems">The items to add</param>
	/// <returns>self</returns>
	public PrioritizedListDictionary<T> AddWithNormalPriority(
		params T[] prioritizedItems)
		=> AddWithPriority(Priority.Normal, prioritizedItems);
}