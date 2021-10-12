// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Rapture.Client;

/// <summary>
/// Dialog utilities
/// </summary>
internal class Dialog
{
    /// <summary>
    /// If the console has been allocated
    /// </summary>
    private static bool s_consoleAllocated = false;

    /// <summary>
    /// Outputs a message
    /// </summary>
    /// <param name="message">The message</param>
    public static void Message(string message)
    {
        AllocateConsole();

        Console.WriteLine(message);
        Console.WriteLine();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }

    /// <summary>
    /// Gets a yes or no reponse
    /// </summary>
    /// <param name="message">The message to display</param>
    /// <returns>The result</returns>
    public static DialogResult YesNo(string message)
    {
        AllocateConsole();

        while (true)
        {
            Console.WriteLine(message);
            Console.Write("(Y/N): ");
            var result = Console.ReadKey();

            Console.Clear();

            if (result.Key == ConsoleKey.Y)
            {
                return DialogResult.Yes;
            }
            else if (result.Key == ConsoleKey.N)
            {
                return DialogResult.No;
            }
        }
    }

    /// <summary>
    /// Gets input
    /// </summary>
    /// <param name="message">The message to display</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns>The input value</returns>
    public static string Input(string message, string defaultValue)
    {
        AllocateConsole();

        Console.WriteLine(message);
        Console.Write($"(default: {defaultValue}): ");
        var input = Console.ReadLine();

        Console.Clear();

        if (string.IsNullOrEmpty(input))
        {
            return defaultValue;
        }
        else
        {
            return input;
        }
    }

    /// <summary>
    /// Allocates a console if it hasnt already
    /// </summary>
    private static void AllocateConsole()
    {
        if (!s_consoleAllocated)
        {
            Utilities.AllocConsole();
            Console.Title = "Rapture Client";
            s_consoleAllocated = true;
        }
    }
}
