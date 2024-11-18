using System;
using Microsoft.AspNetCore.Builder;

namespace Sienar.Infrastructure;

/// <summary>
/// Contains prioritized middlewares for <see cref="SienarWebAppBuilder"/>
/// </summary>
public class MiddlewareProvider : PrioritizedListDictionary<Action<WebApplication>>;