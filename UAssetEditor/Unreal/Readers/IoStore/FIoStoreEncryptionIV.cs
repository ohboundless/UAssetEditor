using UAssetEditor.Binary;

namespace UAssetEditor.Unreal.Readers.IoStore;

public enum EIoEncryptionMethod : byte
{
    None	= 0,
    AES 	= (1 << 0),	// ECB
    AES_CTR	= (1 << 1)
}

public class FIoStoreEncryptionIV
{
    public const int Size = 12;
    public byte[] Bytes;

    public FIoStoreEncryptionIV(Reader Ar)
    {
        Bytes = Ar.ReadBytes(Size);
    }

    public FIoStoreEncryptionIV(byte[] bytes)
    {
        if (bytes.Length != Size)
            throw new ArgumentException($"Encryption IV must be exactly {Size} bytes.", nameof(bytes));
        Bytes = bytes;
    }
}