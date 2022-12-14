namespace TheoryOfAlg_lab3;

public class StringHash
{
    private string _data;

    public StringHash(string data)
    {
        _data = data;
    }

    public override string ToString()
        => _data;

    public override bool Equals(object? other)
        => _data == other as string;

    public override int GetHashCode()
    {
        char[] dataChar = _data.ToCharArray();
        int hash = 7;
        for (int i = 0; i < _data.Length; i++) 
            hash = hash * 31 + dataChar[i];

        return hash;
    }
}