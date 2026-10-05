using System.Data;
using OodleDotNet;

namespace UAssetEditor.Compression;

public static class Oodle
{
    private static OodleDotNet.Oodle? _library;

    public static void Initialize(string dllPath)
    {
        _library = new OodleDotNet.Oodle(dllPath);
    }

    public static byte[] Compress(byte[] data)
    {
        if (_library is null)
            throw new NoNullAllowedException("Oodle library must be initialized!");

        var maxSize = _library.GetCompressedBufferSizeNeeded(OodleCompressor.Leviathan, data.LongLength);
        var buffer = new byte[maxSize];

        var compressedSize = (int)_library.Compress(OodleCompressor.Leviathan, OodleCompressionLevel.Max, data, buffer);
        var result = new byte[compressedSize];
        Buffer.BlockCopy(buffer, 0, result, 0, compressedSize);

        return result;
    }
    
    public static byte[] Decompress(byte[] data, int uncompressedSize)
    {
        if (_library is null)
            throw new NoNullAllowedException("Oodle library must be initialized!");
        
        var result = new byte[uncompressedSize];
        _library.Decompress(data, 0, data.Length, result, 0, result.Length);

        return result;
    }
} 