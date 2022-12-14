namespace TheoryOfAlg_lab3.UserData;

public class Metadata
{
    public const int MetaBlockBytes = 10;
    public readonly ushort HashValue;
    public readonly ulong Offset;

    public Metadata(ushort hashValue, ulong offset)
    {
        HashValue = hashValue;
        Offset = offset;
    }

    public byte[] Serialize()
    {
        var convertShort = BitConverter.GetBytes(HashValue);
        var convertLong = BitConverter.GetBytes(Offset);
        byte[] output = new byte[convertShort.Length + convertLong.Length];
        Buffer.BlockCopy(convertShort, 0, output, 0,convertShort.Length);
        Buffer.BlockCopy(convertLong, 0, output, convertShort.Length,convertLong.Length);

        return output;
    }

    public override string ToString()
        => $"HashValue: {HashValue} | Offset: {Offset}";

    public static Metadata Deserialize(byte[] deserializedMetaBlock)
        => new Metadata(BitConverter.ToUInt16(deserializedMetaBlock, 0),
            BitConverter.ToUInt64(deserializedMetaBlock, 2));
}