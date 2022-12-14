namespace TheoryOfAlg_lab3.UserData;

public class User
{
    public const int SerializedBytesPerUser = 100;
    private const int AgeShift = 0;
    private const int NameShift = 1;
    private const int EmailShift = 50;

    public readonly string Name;
    public readonly string Email;
    public readonly byte Age;

    public User(string name, string email, byte age)
    {
        Name = name;
        Email = email;
        Age = age;
    }

    public override string ToString()
        => $"Name: {Name} | Email: {Email} | Age: {Age}";

    public byte[] Serialize()
    {
        byte[] outputBytes = new byte[SerializedBytesPerUser];
        outputBytes[0] = Age;

        outputBytes[NameShift] = (byte) Name.Length;
        var nameChar = Name.ToCharArray();
        for (int i = 0; i < Name.Length; i++)
            outputBytes[NameShift + 1 + i] = (byte) nameChar[i];

        outputBytes[EmailShift] = (byte) Email.Length;
        var emailChar = Email.ToCharArray();
        for (int i = 0; i < Email.Length; i++)
            outputBytes[EmailShift + 1 + i] = (byte) emailChar[i];

        return outputBytes;
    }

    public static User Deserialize(byte[] serializedUser)
    {
        byte userAge = serializedUser[AgeShift];
        string username = string.Concat(Enumerable.
            Range(0, serializedUser[NameShift]).
            Select(i => (char)serializedUser[NameShift + i + 1]));
        string userEmail = string.Concat(Enumerable.
            Range(0, serializedUser[EmailShift]).
            Select(i => (char)serializedUser[EmailShift + i + 1]));
        
        return new User(username, userEmail, userAge);
    }

    public override bool Equals(object? other)
    {
        User otherUser = (other as User)!;
        return Email == otherUser.Email 
               && Age == otherUser.Age 
               && Name == otherUser.Name;
    }

    public override int GetHashCode()
        => Email.GetHashCode();
}