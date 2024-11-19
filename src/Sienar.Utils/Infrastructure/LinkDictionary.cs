namespace Sienar.Infrastructure;

/// <summary>
/// A dictionary that contains a list of links at different <see cref="Priority"/> keys
/// </summary>
/// <typeparam name="TLink">the type of the link</typeparam>
public class LinkDictionary<TLink> : PrioritizedListDictionary<TLink>;