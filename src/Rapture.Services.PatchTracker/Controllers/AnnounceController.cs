// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net;
using Microsoft.AspNetCore.Mvc;
using MonoTorrent.BEncoding;

namespace Rapture.Services.PatchTracker.Controllers;

/// <summary>
/// Provides patch server tracking for clients
/// </summary>
[Route("announce")]
public class AnnounceController : Controller
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<AnnounceController> _logger;

    /// <summary>
    /// The configuration
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Creates the announce controller
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="configuration">The application configuration</param>
    public AnnounceController(ILogger<AnnounceController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Gets the list of servers that provide patches
    /// </summary>
    [HttpGet]
    public async Task Announce()
    {
        var response = new BEncodedDictionary
        {
            { "tracker id", new BEncodedString("SQ0001-DcPDIHCph") },
            { "interval", new BEncodedNumber(2700) },
            { "min interval", new BEncodedNumber(600) },
            { "complete", new BEncodedNumber(1) },
            { "incomplete", new BEncodedNumber(0) },
            { "downloaded", new BEncodedNumber(0) }
        };

        var patchServersValue = _configuration["PatchServers"];

        if (string.IsNullOrEmpty(patchServersValue))
        {
            _logger.LogError("PatchServers is null or empty!");
            Response.StatusCode = 500;
            return;
        }

        var patchServers = patchServersValue.Split(';');

        var peers = new List<byte>();

        foreach (var patchServer in patchServers)
        {
            if (IPEndPoint.TryParse(patchServer, out var ipEndPoint))
            {
                var address = ipEndPoint.Address.GetAddressBytes();
                var port = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)ipEndPoint.Port));
                var entry = new byte[address.Length + port.Length];

                Array.Copy(address, entry, address.Length);
                Array.Copy(port, 0, entry, address.Length, port.Length);

                peers.AddRange(entry);
            }
            else
            {
                _logger.LogError("PatchServers has invalid value! PatchServers: {PatchServersValue}", patchServersValue);
                Response.StatusCode = 500;
                return;
            }
        }

        response.Add("peers", (BEncodedString)peers.ToArray());

        Response.StatusCode = 200;
        Response.Headers.Add("Content-Type", "text/plain");
        Response.Headers.Add("Connection", "close");

        await Response.Body.WriteAsync(response.Encode());
    }
}
