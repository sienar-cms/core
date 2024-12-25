using System;
using Sienar.Plugins;

namespace Sienar.Infrastructure;

/// <summary>
/// Marks a method as suitable for configuring the <see cref="SienarAppBuilder"/> during app creation
/// </summary>
/// <remarks>
/// This attribute is intended for use by inheritors of <see cref="IPlugin"/>. Each <see cref="IPlugin"/> may decorate a single <c>public static</c> method with <c>[AppConfigurer]</c>. That method must accept a single argument of type <see cref="SienarAppBuilder"/>. Internally, Sienar only checks for the existence of a single `public static` method decorated with <c>[AppConfigurer]</c>, so the behavior of decorating multiple methods is undefined. There is, therefore, no way of guaranteeing which method is called.-
/// </remarks>
[AttributeUsage(AttributeTargets.Method)]
public class AppConfigurerAttribute : Attribute;
