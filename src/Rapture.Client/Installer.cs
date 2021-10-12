// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Win32;

namespace Rapture.Client;

/// <summary>
/// The installer
/// </summary>
internal class Installer
{
    /// <summary>
    /// The boot registry key
    /// </summary>
    private const string BootRegistryKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\ffxivboot.exe";

    /// <summary>
    /// The patch registry key
    /// </summary>
    private const string PatchRegistryKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\ffxivpatch.exe";

    /// <summary>
    /// Checks if the client is installed
    /// </summary>
    /// <returns>If the client is installed</returns>
    public static bool IsInstalled()
    {
        return InstallKeyCorrect(BootRegistryKey) && InstallKeyCorrect(PatchRegistryKey);
    }

    /// <summary>
    /// Installs the client
    /// </summary>
    public static void Install()
    {
        InstallKey(BootRegistryKey);
        InstallKey(PatchRegistryKey);
    }

    /// <summary>
    /// Uninstalls the client
    /// </summary>
    public static void Uninstall()
    {
        UninstallKey(BootRegistryKey);
        UninstallKey(PatchRegistryKey);
        ServerManager.ClearAddress();
    }

    /// <summary>
    /// Checks if an install key is correct
    /// </summary>
    /// <param name="keyPath">The key path</param>
    /// <returns>If the install key is correct</returns>
    private static bool InstallKeyCorrect(string keyPath)
    {
        var key = Registry.LocalMachine.OpenSubKey(keyPath);

        if (key != null)
        {
            var value = key.GetValue("Debugger");

            if (value != null)
            {
                var debugger = (string)value;

                if (debugger == Environment.ProcessPath)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Installs a registry key
    /// </summary>
    /// <param name="keyPath">The key path</param>
    private static void InstallKey(string keyPath)
    {
        var key = Registry.LocalMachine.OpenSubKey(keyPath, true);

        if (key == null)
        {
            key = Registry.LocalMachine.CreateSubKey(keyPath, true);
        }

        var value = key.GetValue("Debugger");

        if (value != null)
        {
            key.DeleteValue("Debugger");
        }

        if (Environment.ProcessPath != null)
        {
            key.SetValue("Debugger", Environment.ProcessPath);
        }
    }

    /// <summary>
    /// Uninstalls a registry key
    /// </summary>
    /// <param name="keyPath">The key path</param>
    private static void UninstallKey(string keyPath)
    {
        var key = Registry.LocalMachine.OpenSubKey(keyPath, true);

        if (key != null)
        {
            key.DeleteValue("Debugger");
        }
    }
}
