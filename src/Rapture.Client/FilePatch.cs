// Licensed to the Rapture Contributors under one or more agreements.
// The Rapture Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Rapture.Client;

/// <summary>
/// A patch for a file
/// </summary>
internal class FilePatch
{
    /// <summary>
    /// The hash of the file
    /// </summary>
    public string FileHash { get; set; } = "";

    /// <summary>
    /// The resulting file hash
    /// </summary>
    public string ResultHash { get; set; } = "";

    /// <summary>
    /// The offset to patch at
    /// </summary>
    public int PatchOffset { get; set; } = 0x0;

    /// <summary>
    /// The patch data
    /// </summary>
    public byte[] PatchData { get; set; } = Array.Empty<byte>();
}
