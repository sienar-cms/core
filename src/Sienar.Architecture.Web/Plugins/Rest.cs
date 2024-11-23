using System;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sienar.Infrastructure;

namespace Sienar.Plugins;

/// <summary>
/// Configures RESTful ASP.NET architecture in a Sienar application, such as ASP.NET controllers and Swagger
/// </summary>
public class Rest : IWebPlugin
{
	/// <inheritdoc />
	public PluginData PluginData { get; } = new()
	{
		Name = "Sienar REST Plugin",
		Description = "Adds Sienar functionality as a RESTful API",
		Author = "Christian LeVesque",
		AuthorUrl = "https://levesque.dev",
		Homepage = "https://sienar.levesque.dev",
		Version = Version.Parse("0.1.0")
	};
}