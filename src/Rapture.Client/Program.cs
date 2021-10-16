// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;
using Rapture.Client.Dialogs;
using Rapture.Client.Patching;
using Rapture.Client.Utilities;

namespace Rapture.Client;

/// <summary>
/// The program
/// </summary>
class Program
{
    /// <summary>
    /// The regex for matching hosts
    /// </summary>
    private static readonly Regex s_addressRegex = new(@"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$");

    /// <summary>
    /// The program entry point
    /// </summary>
    /// <param name="args">The command line arguments</param>
    public static void Main(string[] args)
    {
        if (!Utility.IsAdministrator())
        {
            Utility.ElevatePrivileges(args);
            return;
        }

        if (args.Length == 0)
        {
            var isInstalled = Installer.IsInstalled();

            if (!isInstalled)
            {
                var result = Dialog.YesNo("Rapture is not currently installed, would you like to install it?");

                if (result == DialogResult.Yes)
                {
                    Installer.Install();
                    isInstalled = true;
                    Dialog.Message("Rapture successfully installed!");
                }
                else
                {
                    return;
                }
            }
            else
            {
                var result = Dialog.YesNo("Rapture is currently installed, would you like to uninstall it?");

                if (result == DialogResult.Yes)
                {
                    Installer.Uninstall();
                    isInstalled = false;
                    Dialog.Message("Rapture successfully uninstalled!");
                }
            }

            if (isInstalled)
            {
                var address = Dialog.Input("Please enter the IP address of the Rapture server:", ServerManager.GetAddress());

                var result = s_addressRegex.Match(address);

                if (!result.Success)
                {
                    Dialog.Message("Invalid server address!");
                    return;
                }

                ServerManager.SetAddress(address);
                Dialog.Message("Server address succesfully set!");
            }
        }
        else if (args.Length >= 1)
        {
            if (File.Exists(args[0]))
            {
                var execute = Patcher.PatchFile(args[0]);

                if (args.Length > 1)
                {
                    Utility.StartDebugProcess(execute, args[1..]);
                }
                else
                {
                    Utility.StartDebugProcess(execute);
                }
            }
            else
            {
                Dialog.Message($"{args[0]} does not exist!");
            }
        }
    }
}
