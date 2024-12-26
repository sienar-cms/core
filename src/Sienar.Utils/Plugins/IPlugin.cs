namespace Sienar.Plugins;

/// <summary>
/// Represents a distributable plugin for Sienar applications
/// </summary>
public interface IPlugin
{
	/// <summary>
	/// Configures the application according to the plugin's requirements
	/// </summary>
	void Configure();
}
