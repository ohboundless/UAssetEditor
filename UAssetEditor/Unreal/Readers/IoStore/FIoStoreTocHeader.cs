using UAssetEditor.Binary;
using UAssetEditor.Unreal.Misc;

namespace UAssetEditor.Unreal.Readers.IoStore;

public enum EIoStoreTocVersion : byte
{
    Invalid = 0,
    Initial,
    DirectoryIndex,
    PartitionSize,
    PerfectHash,
    PerfectHashWithOverflow,
    OnDemandMetaData,
    RemovedOnDemandMetaData,
    ReplaceIoChunkHashWithIoHash,
    LatestPlusOne,
    Latest = LatestPlusOne - 1
}

public enum EIoContainerFlags : byte
{
    None,
    Compressed	= (1 << 0),
    Encrypted	= (1 << 1),
    Signed		= (1 << 2),
    Indexed		= (1 << 3),
    OnDemand	= (1 << 4),
}

public class FIoStoreTocHeader
{
    public static byte[] TOC_MAGIC = {0x2D, 0x3D, 0x3D, 0x2D, 0x2D, 0x3D, 0x3D, 0x2D, 0x2D, 0x3D, 0x3D, 0x2D, 0x2D, 0x3D, 0x3D, 0x2D}; // -==--==--==--==-
    
    // public readonly byte[] TocMagic; 
    public readonly EIoStoreTocVersion Version;
    // private readonly byte _reserved0;
    // private readonly ushort _reserved1;
    // public readonly uint TocHeaderSize; Should always be 144
    public readonly uint TocEntryCount;
    public readonly uint TocCompressedBlockEntryCount;
    public readonly uint TocCompressedBlockEntrySize;
    public readonly uint CompressionMethodNameCount;
    public readonly uint CompressionMethodNameLength;
    public readonly uint CompressionBlockSize;
    public readonly uint DirectoryIndexSize;
    public uint PartitionCount;
    public readonly FIoContainerId ContainerId;
    public readonly FGuid EncryptionKeyGuid;
    public readonly EIoContainerFlags ContainerFlags;
    public EIoEncryptionMethod EncryptionMethod;
    //public readonly ushort _reserved4;
    public readonly uint TocChunkPerfectHashSeedsCount;
    public ulong PartitionSize;
    public readonly uint TocChunksWithoutPerfectHashCount;
    public readonly uint TocSourceHashCount;
    public uint EncryptionIVCount;
    //private readonly uint _reserved8;
    //private readonly ulong[] _reserved9;
    
    
    public FIoStoreTocHeader(Reader reader)
    {
        if (!reader.ReadBytes(TOC_MAGIC.Length).SequenceEqual(TOC_MAGIC))
            throw new InvalidDataException("Invalid TOC magic");

        Version = reader.Read<EIoStoreTocVersion>();

        reader.Position += 1 + 2 + sizeof(uint); // Padding + TocHeaderSize

        TocEntryCount = reader.Read<uint>();
        TocCompressedBlockEntryCount = reader.Read<uint>();
        TocCompressedBlockEntrySize = reader.Read<uint>();
        CompressionMethodNameCount = reader.Read<uint>();
        CompressionMethodNameLength = reader.Read<uint>();
        CompressionBlockSize = reader.Read<uint>();
        DirectoryIndexSize = reader.Read<uint>();
        PartitionCount = reader.Read<uint>();
        ContainerId = reader.Read<FIoContainerId>();
        EncryptionKeyGuid = reader.Read<FGuid>();
        ContainerFlags = reader.Read<EIoContainerFlags>();
        EncryptionMethod = reader.Read<EIoEncryptionMethod>();
        reader.Position += 2;
        TocChunkPerfectHashSeedsCount = reader.Read<uint>();
        PartitionSize = reader.Read<ulong>();
        TocChunksWithoutPerfectHashCount = reader.Read<uint>();
        TocSourceHashCount = reader.Read<uint>(); // _reserved7
        EncryptionIVCount = reader.Read<uint>(); // _reserved8[0]
        
        reader.Position = 144; // Should be right always?
    }
}