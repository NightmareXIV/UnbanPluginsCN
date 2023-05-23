using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace UnbanPluginsCN;

/// <summary>
/// Banned plugin version that is blocked from installation.
/// </summary>
internal class BannedPlugin
{
    /// <summary>
    /// Gets plugin name.
    /// </summary>
    [JsonProperty]
    public string Name { get; set; }

    /// <summary>
    /// Gets plugin assembly version.
    /// </summary>
    [JsonProperty]
    public string AssemblyVersion { get; set; }

    /// <summary>
    /// Gets reason for the ban.
    /// </summary>
    [JsonProperty]
    [DefaultValue(null)] public string Reason { get; set; } = null;
}