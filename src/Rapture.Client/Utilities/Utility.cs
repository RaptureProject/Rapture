// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Rapture.Client.Utilities;

/// <summary>
/// General utilities
/// </summary>
internal class Utility
{
    /// <summary>
    /// Allocates a console window
    /// </summary>
    [DllImport("kernel32")]
    public static extern void AllocConsole();

    /// <summary>
    /// Checks if the process is running as an administrator
    /// </summary>
    /// <returns>True if running as an administrator, otherwise false</returns>
    public static bool IsAdministrator()
    {
        using var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    /// <summary>
    /// Elevates the privileges of the current process to run as an admiistrator
    /// </summary>
    /// <param name="args">The command line arguments</param>
    public static void ElevatePrivileges(string[] args)
    {
        if (Environment.ProcessPath != null)
        {
            if (args.Length == 0)
            {
                var startInfo = new ProcessStartInfo()
                {
                    FileName = Environment.ProcessPath,
                    UseShellExecute = true,
                    Verb = "runas"
                };

                Process.Start(startInfo);
            }
            else if (args.Length >= 1)
            {
                if (args[0] == "elevate")
                {
                    var arguments = string.Join(' ', args[1..].Select(a => $"\"{a}\""));

                    var startInfo = new ProcessStartInfo()
                    {
                        FileName = Environment.ProcessPath,
                        Arguments = arguments,
                        UseShellExecute = true,
                        Verb = "runas"
                    };

                    Process.Start(startInfo);
                }
                else
                {
                    var arguments = string.Join(' ', args[0..].Select(a => $"\"{a}\""));
                    Process.Start(Environment.ProcessPath, $"elevate {arguments}");
                }
            }
        }
    }

    /// <summary>
    /// Starts a debug process
    /// </summary>
    /// <param name="executablePath">The executable path</param>
    public static void StartDebugProcess(string executablePath) => StartDebugProcess(executablePath, new string[] { "" });

    /// <summary>
    /// Starts a debug process
    /// </summary>
    /// <param name="executablePath">The executable path</param>
    /// <param name="args">The command line arguments</param>
    public static void StartDebugProcess(string executablePath, string[] args)
    {
        if (Environment.ProcessorCount > 14)
        {
            Process.GetCurrentProcess().ProcessorAffinity = (IntPtr)0x3FFF;
        }

        unsafe
        {
            var fullCommand = $"\"{executablePath}\" {string.Join(' ', args.Select(a => $"\"{a}\""))}";

            fixed (char* commandLine = fullCommand)
            {
                var startupInfo = new Windows.Win32.System.Threading.STARTUPINFOW();
                var creationFlags = Windows.Win32.System.Threading.PROCESS_CREATION_FLAGS.DEBUG_ONLY_THIS_PROCESS;
                Windows.Win32.PInvoke.CreateProcess(executablePath, commandLine, null, null, false, creationFlags, null, null, startupInfo, out var pi);
                Windows.Win32.PInvoke.DebugActiveProcessStop(pi.dwProcessId);
            }
        }
    }
}
