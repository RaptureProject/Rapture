// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;
using System.Text.RegularExpressions;

namespace Rapture.Client.Utilities;

/// <summary>
/// The server manager
/// </summary>
internal class ServerManager
{
    /// <summary>
    /// The path to the hosts file
    /// </summary>
    private const string HostsPath = @"C:\Windows\System32\drivers\etc\hosts";

    /// <summary>
    /// The hosts regex string
    /// </summary>
    private const string HostsRegexString = @"# Start of Rapture.*# Server IP: \(([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3})\).*# End of Rapture";

    /// <summary>
    /// The hosts regex
    /// </summary>
    private static readonly Regex s_hostsRegex = new(HostsRegexString, RegexOptions.Singleline);

    /// <summary>
    /// The list of hosts
    /// </summary>
    private static readonly IReadOnlyList<string> s_hosts = new List<string>()
    {
        "ver01.ffxiv.com",
        "track01.ffxiv.com",
        "account.square-enix.com",
        "lobby01.ffxiv.com"
    };

    /// <summary>
    /// Sets the server address
    /// </summary>
    /// <param name="address">The server address</param>
    public static void SetAddress(string address)
    {
        var builder = new StringBuilder();

        builder.Append($"# Start of Rapture{Environment.NewLine}");
        builder.Append($"# Server IP: ({address}){Environment.NewLine}");

        foreach (var host in s_hosts)
        {
            builder.Append($"{address} {host}{Environment.NewLine}");
        }

        builder.Append($"# End of Rapture");

        var hosts = File.ReadAllText(HostsPath);
        var result = s_hostsRegex.Match(hosts);

        if (result.Success)
        {
            hosts = s_hostsRegex.Replace(hosts, builder.ToString());
        }
        else
        {
            if (hosts.EndsWith(Environment.NewLine))
            {
                hosts += builder.ToString() + Environment.NewLine;
            }
            else
            {
                hosts += Environment.NewLine + builder.ToString() + Environment.NewLine;
            }
        }

        File.WriteAllText(HostsPath, hosts);
    }

    /// <summary>
    /// Gets the server address
    /// </summary>
    /// <returns>The server address</returns>
    public static string GetAddress()
    {
        var hosts = File.ReadAllText(HostsPath);
        var result = s_hostsRegex.Match(hosts);

        if (result.Success)
        {
            return result.Groups[1].Value;
        }
        else
        {
            return "127.0.0.1";
        }
    }

    /// <summary>
    /// Clears the address
    /// </summary>
    public static void ClearAddress()
    {
        var hosts = File.ReadAllText(HostsPath);
        var result = s_hostsRegex.Match(hosts);

        if (result.Success)
        {
            hosts = s_hostsRegex.Replace(hosts, "");

            if (hosts.EndsWith(Environment.NewLine + Environment.NewLine))
            {
                hosts = hosts[..^Environment.NewLine.Length];
            }

            File.WriteAllText(HostsPath, hosts);
        }
    }
}
