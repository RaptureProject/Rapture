// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Security.Cryptography;

namespace Rapture.Client;

/// <summary>
/// The patcher
/// </summary>
internal class Patcher
{
    /// <summary>
    /// The patch file name
    /// </summary>
    private const string PatchFileName = "ffxivpatch.exe";

    /// <summary>
    /// The list of file patches
    /// </summary>
    private static readonly IReadOnlyList<FilePatch> s_filePatches = new List<FilePatch>()
    {
        // 2010.07.10.0000 ffxivboot.exe
        new()
        {
            FileHash = "A5A8F843389D97A2DDAA081B1D571ABE0E7BAAD24FA55B87660B7321ACF5ED35",
            ResultHash = "8C9A9A8580FA429238CAEB63F31B3221A1AF9B4E6F6E935DA6D3345A9F03F6B7",
            PatchOffset = 0x5DF64,
            PatchData = new byte[] { 0x01 }
        },

        // 2010.09.18.0000 ffxivboot.exe
        new()
        {
            FileHash = "6A18533D4C3B296CCDEDD84C81A3EB99AE5DDB47C3416DE60E3414983783EFEF",
            ResultHash = "E0531FA034B2A38138930131184C80C6CC57618468AE40E258C73223F54948F1",
            PatchOffset = 0x646EF,
            PatchData = new byte[] { 0x19 }
        }
    };

    /// <summary>
    /// Patches a file
    /// </summary>
    /// <param name="filePath">The path to the file</param>
    /// <returns>The final file path</returns>
    /// <exception cref="FileNotFoundException">Occurs when the specified file path doesnt exist</exception>
    public static string PatchFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(filePath);
        }

        if (Path.GetFileName(filePath) == PatchFileName)
        {
            return filePath;
        }

        var baseDirectory = Path.GetDirectoryName(filePath);

        if (baseDirectory == null)
        {
            return filePath;
        }

        var hash = HashFile(filePath);
        var patch = s_filePatches.Where(p => p.FileHash == hash).FirstOrDefault();

        if (patch == null)
        {
            return filePath;
        }

        var patchPath = Path.Combine(baseDirectory, PatchFileName);

        if (File.Exists(patchPath))
        {
            if (patch.ResultHash == HashFile(patchPath))
            {
                return patchPath;
            }
            else
            {
                File.Delete(patchPath);
            }
        }

        File.Copy(filePath, patchPath);

        using var file = new FileStream(patchPath, FileMode.Open, FileAccess.ReadWrite);
        file.Seek(patch.PatchOffset, SeekOrigin.Begin);
        file.Write(patch.PatchData);
        file.Flush();

        return patchPath;
    }

    /// <summary>
    /// Hashes a file
    /// </summary>
    /// <param name="path">The file path</param>
    /// <returns>The file hash</returns>
    private static string HashFile(string path)
    {
        using var file = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(file);
        return Convert.ToHexString(hash);
    }
}
