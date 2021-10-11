// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Mvc;
using Rapture.Services.VersionCheck.Data;
using Rapture.Services.VersionCheck.Models;

namespace Rapture.Services.VersionCheck.Controllers;

/// <summary>
/// Provides version checking for clients
/// </summary>
[Route("patch/vercheck/ffxiv/win32/release")]
public class VersionCheckController : Controller
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<VersionCheckController> _logger;

    /// <summary>
    /// The version repository
    /// </summary>
    private readonly VersionRepository _versionRepository;

    /// <summary>
    /// Creates the version check controller
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="versionRepository">The version repository</param>
    public VersionCheckController(ILogger<VersionCheckController> logger, VersionRepository versionRepository)
    {
        _logger = logger;
        _versionRepository = versionRepository;
    }

    /// <summary>
    /// Checks the version of a client
    /// </summary>
    /// <param name="type">The type of patch</param>
    /// <param name="version">The version</param>
    /// <response code="204">Up to date</response>
    /// <response code="404">Version not found</response>
    [HttpGet("{type}/{version}")]
    public async Task VersionCheck(VersionType type, string version)
    {
        if (!_versionRepository.VersionExists(type, version))
        {
            Response.StatusCode = 404;
            return;
        }

        var latestVersion = _versionRepository.GetLatestVersion(type);
        var versionTypeHash = _versionRepository.GetVersionTypeHash(type);

        Response.Headers.Add("Content-Location", $"ffxiv/{versionTypeHash}/vercheck.dat");
        Response.Headers.Add("X-Repository", $"ffxiv/win32/release/{type.ToString().ToLower()}");
        Response.Headers.Add("X-Patch-Module", "ZiPatch");
        Response.Headers.Add("X-Protocol", "torrent");
        Response.Headers.Add("X-Info-Url", "http://example.com");
        Response.Headers.Add("X-Latest-Version", latestVersion);

        if (version == latestVersion)
        {
            Response.StatusCode = 204;
        }
        else
        {
            var updateVersions = _versionRepository.GetUpdateVersions(type, version);

            foreach (var updateVersion in updateVersions)
            {
                if (!System.IO.File.Exists(_versionRepository.GetVersionTorrentPath(updateVersion.Type, updateVersion.Version)))
                {
                    var errorType = updateVersion.Type.ToString().ToLower();
                    var errorVersion = updateVersion.Version;

                    _logger.LogError("Torrent for {ErrorType} patch {ErrorVersion} is missing!", errorType, errorVersion);
                    Response.StatusCode = 500;
                    return;
                }
            }

            Response.StatusCode = 200;
            Response.Headers.Add("Content-Type", "multipart/mixed; boundary=477D80B1_38BC_41d4_8B48_5273ADB89CAC");
            Response.Headers.Add("Connection", "keep-alive");

            using var writer = new StreamWriter(Response.Body, leaveOpen: true);

            foreach (var updateVersion in updateVersions)
            {
                using var reader = System.IO.File.OpenRead(_versionRepository.GetVersionTorrentPath(updateVersion.Type, updateVersion.Version));

                await writer.WriteAsync("--477D80B1_38BC_41d4_8B48_5273ADB89CAC\r\n");
                await writer.WriteAsync("Content-Type: application/octet-stream\r\n");
                await writer.WriteAsync($"Content-Location: ffxiv/{versionTypeHash}/metainfo/D{updateVersion.Version}.torrent\r\n");
                await writer.WriteAsync($"X-Patch-Length: {updateVersion.PatchLength}\r\n");
                await writer.WriteAsync("X-Signature: jqxmt9WQH1aXptNju6CmCdztFdaKbyOAVjdGw_DJvRiBJhnQL6UlDUcqxg2DeiIKhVzkjUm3hFXOVUFjygxCoPUmCwnbCaryNqVk_oTk_aZE4HGWNOEcAdBwf0Gb2SzwAtk69zs_5dLAtZ0mPpMuxWJiaNSvWjEmQ925BFwd7Vk=\r\n");
                await writer.WriteAsync("\r\n");
                await writer.FlushAsync();
                await reader.CopyToAsync(writer.BaseStream);
            }

            await writer.WriteAsync("--477D80B1_38BC_41d4_8B48_5273ADB89CAC--\r\n\r\n");
        }
    }
}
