// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Rapture.Services.VersionCheck.Models;

/// <summary>
/// Contains info on a specific version
/// </summary>
public class VersionInfo
{
    /// <summary>
    /// The version type
    /// </summary>
    public VersionType Type { get; set; } = VersionType.Boot;

    /// <summary>
    /// The version number
    /// </summary>
    public string Version { get; set; } = "";

    /// <summary>
    /// The date this version was built on
    /// </summary>
    public DateTime BuildDate { get; set; } = DateTime.UnixEpoch;

    /// <summary>
    /// The length of the patch
    /// </summary>
    public int PatchLength { get; set; } = 0;
}
