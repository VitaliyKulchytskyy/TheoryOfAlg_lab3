namespace TheoryOfAlg_lab3;

public class Key
{
    private readonly string _stock;
    private readonly int _dayOfYear;

    public Key(string stock, int dayOfYear)
    {
        _stock = stock;
        _dayOfYear = dayOfYear;
    }

    public override int GetHashCode()
        => _dayOfYear;

    public override bool Equals(object? other)
    {
        Key otherKey = (other as Key)!;
        return otherKey._dayOfYear == this._dayOfYear
               && otherKey._stock == this._stock;
    }
}