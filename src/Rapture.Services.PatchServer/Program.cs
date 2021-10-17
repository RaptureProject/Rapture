// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using MonoTorrent.Client;
using Rapture.Services.PatchServer;
using Rapture.Services.PatchServer.Data;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices(services =>
{
    services.AddSingleton<InfoHashRepository>();

    services.AddSingleton<ClientEngine>((services) =>
    {
        var configuration = services.GetRequiredService<IConfiguration>();
        var infoHashRepository = services.GetRequiredService<InfoHashRepository>();

        var settingsBuilder = new EngineSettingsBuilder()
        {
            AllowLocalPeerDiscovery = false,
            AllowPortForwarding = false,
            AutoSaveLoadDhtCache = false,
            AutoSaveLoadFastResume = false,
            AutoSaveLoadMagnetLinkMetadata = false,
            DhtPort = -1,
            ListenPort = configuration.GetValue<int>("ServerPort"),
            RapturePeerId = infoHashRepository.PeerId,
            RaptureInfoHashes = infoHashRepository.InfoHashes
        };

        return new ClientEngine(settingsBuilder.ToSettings());
    });

    services.AddHostedService<Server>();
});

var app = builder.Build();

await app.RunAsync();
