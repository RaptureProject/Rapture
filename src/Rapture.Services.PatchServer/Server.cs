// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using MonoTorrent;
using MonoTorrent.Client;

namespace Rapture.Services.PatchServer;

/// <summary>
/// The server that hosts the client engine
/// </summary>
public class Server : BackgroundService
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<Server> _logger;

    /// <summary>
    /// The client engine
    /// </summary>
    private readonly ClientEngine _clientEngine;

    /// <summary>
    /// The host environment
    /// </summary>
    private readonly IHostEnvironment _env;

    /// <summary>
    /// The new line string
    /// </summary>
    private readonly string _newLine = Environment.NewLine;

    /// <summary>
    /// Creates an instance of the server
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="clientEngine">The client engine</param>
    /// <param name="env">The hosting environment</param>
    public Server(ILogger<Server> logger, ClientEngine clientEngine, IHostEnvironment env)
    {
        _logger = logger;
        _clientEngine = clientEngine;
        _env = env;
    }

    /// <summary>
    /// Executes the server
    /// </summary>
    /// <param name="stoppingToken">The stopping token</param>
    /// <returns>A task</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _clientEngine.CriticalException += EngineCriticalException;

        var dataPath = Path.Combine(_env.ContentRootPath, "PatchData", "ffxiv");

        foreach (var file in Directory.GetFiles(dataPath, "*.torrent", SearchOption.AllDirectories))
        {
            var torrent = await Torrent.LoadAsync(file);
            var manager = await _clientEngine.AddAsync(torrent, dataPath);

            manager.TorrentStateChanged += PatchStateChanged;
            manager.PeerConnected += PeerConnected;
            manager.PeerDisconnected += PeerDisconnected;

            await manager.StartAsync();
        }

        var initialized = false;

        while (_clientEngine.IsRunning)
        {
            if (!initialized)
            {
                var ready = _clientEngine.Torrents.All(t => t.State == TorrentState.Seeding);

                if (ready)
                {
                    initialized = true;
                    LogReady();
                }
                else
                {
                    LogInitialize();
                }
            }

            await Task.Delay(5000, stoppingToken);
        }

        foreach (var manager in _clientEngine.Torrents)
        {
            var file = Path.GetFileName(manager.Files.First().Path);
            var stop = manager.StopAsync();

            while (manager.State != TorrentState.Stopped)
            {
                _logger.LogInformation("Stopping {File}", file);
                await stop;
            }

            await stop;
        }
    }

    /// <summary>
    /// Occurs when there is a critical exception in the engine
    /// </summary>
    /// <param name="sender">The sending object</param>
    /// <param name="e">The arguments</param>
    private void EngineCriticalException(object? sender, CriticalExceptionEventArgs e)
    {
        _logger.LogError(e.Exception, "Critical exception occured in client engine!");
    }

    /// <summary>
    /// Logs initialization status
    /// </summary>
    private void LogInitialize()
    {
        var initialize = $"Initialize:{_newLine}";
        var hashing = $"Hashing: {_clientEngine.Torrents.Where(t => t.State == TorrentState.Hashing).Count()}{_newLine}";
        var error = $"Error: {_clientEngine.Torrents.Where(t => t.State == TorrentState.Error).Count()}{_newLine}";
        var downloading = $"Downloading: {_clientEngine.Torrents.Where(t => t.State == TorrentState.Downloading).Count()}{_newLine}";
        var seeding = $"Seeding: {_clientEngine.Torrents.Where(t => t.State == TorrentState.Seeding).Count()}{_newLine}";
        var stopped = $"Stopped: {_clientEngine.Torrents.Where(t => t.State == TorrentState.Stopped).Count()}";

        _logger.LogInformation("{Initialize}{Hashing}{Error}{Downloading}{Seeding}{Stopped}", initialize, hashing, error, downloading, seeding, stopped);
    }

    /// <summary>
    /// Logs ready status
    /// </summary>
    private void LogReady()
    {
        var patchCount = _clientEngine.Torrents.Count;

        _logger.LogInformation("Patch server ready. Serving {PatchCount} patches.", patchCount);
    }

    /// <summary>
    /// Occurs when patch state has changes
    /// </summary>
    /// <param name="sender">The sending object</param>
    /// <param name="e">The arguments</param>
    private void PatchStateChanged(object? sender, TorrentStateChangedEventArgs e)
    {
        var patchStateChanged = $"PatchStateChanged:{_newLine}";
        var infoHash = $"InfoHash: {e.TorrentManager.InfoHash.ToHex()}{_newLine}";
        var file = $"File: {Path.GetFileName(e.TorrentManager.Files.First().Path)}{_newLine}";
        var oldState = $"OldState: {e.OldState}{_newLine}";
        var newState = $"NewState: {e.NewState}";

        _logger.LogInformation("{PatchStateChanged}{InfoHash}{File}{OldState}{NewState}", patchStateChanged, infoHash, file, oldState, newState);
    }

    /// <summary>
    /// Occurs when a peer has connected
    /// </summary>
    /// <param name="sender">The sending object</param>
    /// <param name="e">The arguments</param>
    private void PeerConnected(object? sender, PeerConnectedEventArgs e)
    {
        var peerConnected = $"PeerConnected:{_newLine}";
        var peerId = $"PeerId: {e.Peer.PeerID}{_newLine}";
        var peerAddress = $"PeerAddress: {e.Peer.Uri}{_newLine}";
        var infoHash = $"InfoHash: {e.TorrentManager.InfoHash.ToHex()}{_newLine}";
        var file = $"File: {Path.GetFileName(e.TorrentManager.Files.First().Path)}";

        _logger.LogInformation("{PeerConnected}{PeerId}{PeerAddress}{InfoHash}{File}", peerConnected, peerId, peerAddress, infoHash, file);
    }

    /// <summary>
    /// Occurs when a peer has disconnected
    /// </summary>
    /// <param name="sender">The sending object</param>
    /// <param name="e">The arguments</param>
    private void PeerDisconnected(object? sender, PeerDisconnectedEventArgs e)
    {
        var peerDisconnected = $"PeerDisconnected:{_newLine}";
        var peerId = $"PeerId: {e.Peer.PeerID}{_newLine}";
        var peerAddress = $"PeerAddress: {e.Peer.Uri}{_newLine}";
        var infoHash = $"InfoHash: {e.TorrentManager.InfoHash.ToHex()}{_newLine}";
        var file = $"File: {Path.GetFileName(e.TorrentManager.Files.First().Path)}";

        _logger.LogInformation("{PeerDisconnected}{PeerId}{PeerAddress}{InfoHash}{File}", peerDisconnected, peerId, peerAddress, infoHash, file);
    }
}
