// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Rapture.Services.VersionCheck.Models;

namespace Rapture.Services.VersionCheck.Data;

/// <summary>
/// The repository of version info
/// </summary>
public class VersionRepository
{
    /// <summary>
    /// The host environment
    /// </summary>
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// Creates an instance of the version info repository
    /// </summary>
    /// <param name="env">The environment</param>
    public VersionRepository(IWebHostEnvironment env)
    {
        _env = env;
    }

    /// <summary>
    /// All of the versions
    /// </summary>
    private readonly IReadOnlyList<VersionInfo> _versions = new List<VersionInfo>()
    {
        new VersionInfo { Type = VersionType.Boot, Version = "2010.07.10.0000", BuildDate = new DateTime(2010, 07, 10), PatchLength = 0 },
        new VersionInfo { Type = VersionType.Boot, Version = "2010.09.18.0000", BuildDate = new DateTime(2010, 09, 18), PatchLength = 5571687 },
        new VersionInfo { Type = VersionType.Game, Version = "2010.07.10.0000", BuildDate = new DateTime(2010, 07, 10), PatchLength = 0 },
        new VersionInfo { Type = VersionType.Game, Version = "2010.09.19.0000", BuildDate = new DateTime(2010, 09, 19), PatchLength = 444398866 },
        new VersionInfo { Type = VersionType.Game, Version = "2010.09.23.0000", BuildDate = new DateTime(2010, 09, 23), PatchLength = 6907277 },
        new VersionInfo { Type = VersionType.Game, Version = "2010.09.28.0000", BuildDate = new DateTime(2010, 09, 28), PatchLength = 18803280 },
        new VersionInfo { Type = VersionType.Game, Version = "2010.10.07.0001", BuildDate = new DateTime(2010, 10, 07), PatchLength = 19226330 },
        new VersionInfo { Type = VersionType.Game, Version = "2010.10.14.0000", BuildDate = new DateTime(2010, 10, 14), PatchLength = 19464329 },
        new VersionInfo { Type = VersionType.Game, Version = "2010.10.22.0000", BuildDate = new DateTime(2010, 10, 22), PatchLength = 19778252 },
        new VersionInfo { Type = VersionType.Game, Version = "2010.10.26.0000", BuildDate = new DateTime(2010, 10, 26), PatchLength = 19778391 },
        new VersionInfo { Type = VersionType.Game, Version = "2010.11.25.0002", BuildDate = new DateTime(2010, 11, 25), PatchLength = 250718651 },
        new VersionInfo { Type = VersionType.Game, Version = "2010.11.30.0000", BuildDate = new DateTime(2010, 11, 30), PatchLength = 6921623 },
        new VersionInfo { Type = VersionType.Game, Version = "2010.12.06.0000", BuildDate = new DateTime(2010, 12, 06), PatchLength = 7158904 },
        new VersionInfo { Type = VersionType.Game, Version = "2010.12.13.0000", BuildDate = new DateTime(2010, 12, 13), PatchLength = 263311481 },
        new VersionInfo { Type = VersionType.Game, Version = "2010.12.21.0000", BuildDate = new DateTime(2010, 12, 21), PatchLength = 7521358 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.01.18.0000", BuildDate = new DateTime(2011, 01, 18), PatchLength = 9954265 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.02.01.0000", BuildDate = new DateTime(2011, 02, 01), PatchLength = 11632816 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.02.10.0000", BuildDate = new DateTime(2011, 02, 10), PatchLength = 11714096 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.03.01.0000", BuildDate = new DateTime(2011, 03, 01), PatchLength = 77464101 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.03.24.0000", BuildDate = new DateTime(2011, 03, 24), PatchLength = 108923937 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.03.30.0000", BuildDate = new DateTime(2011, 03, 30), PatchLength = 109010880 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.04.13.0000", BuildDate = new DateTime(2011, 04, 13), PatchLength = 341603850 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.04.21.0000", BuildDate = new DateTime(2011, 04, 21), PatchLength = 343579198 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.05.19.0000", BuildDate = new DateTime(2011, 05, 19), PatchLength = 344239925 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.06.10.0000", BuildDate = new DateTime(2011, 06, 10), PatchLength = 344334860 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.07.20.0000", BuildDate = new DateTime(2011, 07, 20), PatchLength = 584926805 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.07.26.0000", BuildDate = new DateTime(2011, 07, 26), PatchLength = 7649141 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.08.05.0000", BuildDate = new DateTime(2011, 08, 05), PatchLength = 152064532 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.08.09.0000", BuildDate = new DateTime(2011, 08, 09), PatchLength = 8573687 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.08.16.0000", BuildDate = new DateTime(2011, 08, 16), PatchLength = 6118907 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.10.04.0000", BuildDate = new DateTime(2011, 10, 04), PatchLength = 677633296 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.10.12.0001", BuildDate = new DateTime(2011, 10, 12), PatchLength = 28941655 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.10.27.0000", BuildDate = new DateTime(2011, 10, 27), PatchLength = 29179764 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.12.14.0000", BuildDate = new DateTime(2011, 12, 14), PatchLength = 374617428 },
        new VersionInfo { Type = VersionType.Game, Version = "2011.12.23.0000", BuildDate = new DateTime(2011, 12, 23), PatchLength = 22363713 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.01.18.0000", BuildDate = new DateTime(2012, 01, 18), PatchLength = 48998794 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.01.24.0000", BuildDate = new DateTime(2012, 01, 24), PatchLength = 49126606 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.01.31.0000", BuildDate = new DateTime(2012, 01, 31), PatchLength = 49536396 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.03.07.0000", BuildDate = new DateTime(2012, 03, 07), PatchLength = 320630782 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.03.09.0000", BuildDate = new DateTime(2012, 03, 09), PatchLength = 8312819 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.03.22.0000", BuildDate = new DateTime(2012, 03, 22), PatchLength = 22027738 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.03.29.0000", BuildDate = new DateTime(2012, 03, 29), PatchLength = 8322920 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.04.04.0000", BuildDate = new DateTime(2012, 04, 04), PatchLength = 8678570 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.04.23.0001", BuildDate = new DateTime(2012, 04, 23), PatchLength = 289511791 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.05.08.0000", BuildDate = new DateTime(2012, 05, 08), PatchLength = 27266546 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.05.15.0000", BuildDate = new DateTime(2012, 05, 15), PatchLength = 27416023 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.05.22.0000", BuildDate = new DateTime(2012, 05, 22), PatchLength = 27742726 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.06.06.0000", BuildDate = new DateTime(2012, 06, 06), PatchLength = 129984024 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.06.19.0000", BuildDate = new DateTime(2012, 06, 19), PatchLength = 133434217 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.06.26.0000", BuildDate = new DateTime(2012, 06, 26), PatchLength = 133581048 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.07.21.0000", BuildDate = new DateTime(2012, 07, 21), PatchLength = 253224781 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.08.10.0000", BuildDate = new DateTime(2012, 08, 10), PatchLength = 42851112 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.09.06.0000", BuildDate = new DateTime(2012, 09, 06), PatchLength = 20566711 },
        new VersionInfo { Type = VersionType.Game, Version = "2012.09.19.0001", BuildDate = new DateTime(2012, 09, 19), PatchLength = 20874726 }
    };

    /// <summary>
    /// The version type hashes
    /// </summary>
    private readonly IReadOnlyDictionary<VersionType, string> _versionTypeHashes = new Dictionary<VersionType, string>()
    {
        { VersionType.Boot, "2d2a390f" },
        { VersionType.Game, "48eca647" }
    };

    /// <summary>
    /// Checks if a version exists
    /// </summary>
    /// <param name="type">The version type</param>
    /// <param name="version">The version</param>
    /// <returns>If the version exists</returns>
    public bool VersionExists(VersionType type, string version)
    {
        return _versions
            .Where(v => v.Type == type && v.Version == version)
            .Any();
    }

    /// <summary>
    /// Gets the latest version of a specified version type
    /// </summary>
    /// <param name="type">The version type</param>
    /// <returns>The latest version for the specified version type</returns>
    public string GetLatestVersion(VersionType type)
    {
        return _versions.Where(v => v.Type == type)
            .OrderBy(v => v.BuildDate)
            .Last()
            .Version;
    }

    /// <summary>
    /// Gets the required versions a client must update to
    /// </summary>
    /// <param name="type">The version type</param>
    /// <param name="version">The base version</param>
    /// <returns>The list of patches that come after the specified patch</returns>
    public IEnumerable<VersionInfo> GetUpdateVersions(VersionType type, string version)
    {
        var baseVersionInfo = _versions
            .Where(v => v.Type == type && v.Version == version)
            .First();

        return _versions
            .Where(v => v.Type == type && v.BuildDate > baseVersionInfo.BuildDate)
            .OrderBy(v => v.BuildDate);
    }

    /// <summary>
    /// Gets the path to the torrent file for a version
    /// </summary>
    /// <param name="type">The version type</param>
    /// <param name="version">The version</param>
    /// <returns></returns>
    public string GetVersionTorrentPath(VersionType type, string version)
    {
        return Path.Combine(_env.ContentRootPath, "PatchData", "ffxiv", _versionTypeHashes[type], "metainfo", $"D{version}.torrent");
    }

    /// <summary>
    /// Gets a version type hash
    /// </summary>
    /// <param name="type">The version type</param>
    /// <returns>The version type hash</returns>
    public string GetVersionTypeHash(VersionType type)
    {
        return _versionTypeHashes[type];
    }
}
