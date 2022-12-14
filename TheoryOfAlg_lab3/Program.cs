using System.Collections;
using Microsoft.Win32.SafeHandles;
using TheoryOfAlg_lab3.UserData;

namespace TheoryOfAlg_lab3;

public static class Program
{
    private const string Path = "/home/karlenko/projects/csharp/TheoryOfAlg_lab3/TheoryOfAlg_lab3/UserData/UsersDataBase.db";
    private const int UserAmount = 10000;
    
    static void GenerateDataBase(int amountOfUser = UserAmount, string path = Path)
    {
        var hashTable = new HashTable<StringHash, User>(UserAmount, false);
        for (int i = 0; i < amountOfUser; i++)
        {
            StringHash genEmail = new StringHash("email" + i + "@gmail.com");
            hashTable[genEmail] = new User("User" + i , genEmail.ToString(), (byte) (1 + i % 87));
        }
        
        //hashTable.PrintBuckets();

        using (var file = File.OpenWrite(path))
        {
            foreach (User elem in hashTable)
            {
                file.Write(elem.Serialize());
            }
        }
        
        Console.WriteLine("[?] DataBase successfully generated.");
    }

    static void ReadFromDataBase(int amountOfUser, string path = Path)
    {
        using (var file = File.OpenRead(path))
        {
            byte[] buffer = new byte[User.SerializedBytesPerUser];
            for (int i = 0; i < amountOfUser; i++)
            {
                file.Read(buffer);
                Console.WriteLine(User.Deserialize(buffer));
            }
        }
    }

    static User RandomAccessFromDataBase(int userIndex, string path = Path)
    {
        using (var file = new BinaryReader(File.OpenRead(path)))
        {
            file.BaseStream.Seek(userIndex * User.SerializedBytesPerUser, SeekOrigin.Begin);
         
            byte[] buffer = new byte[User.SerializedBytesPerUser];
            file.Read(buffer);
            return User.Deserialize(buffer);
        }
    }

    static User? FindUserInfoByEmail(string email)
    {
        StringHash emailHash = new StringHash(email);
        var hashTable = new HashTable<StringHash, User>(UserAmount, false);
        int index = hashTable.CalcIndexByKey(emailHash) < 8 ? hashTable.CalcIndexByKey(emailHash) : hashTable.CalcIndexByKey(emailHash) - 8;
        User output = null;

        do
        {
            output = RandomAccessFromDataBase(index++);
            if (index > UserAmount)
                return null;
        } while (!emailHash.Equals(output.Email)); 

        return output;
    }

    static string GetUserInfoByEmail(string email)
    {
        User? findUserByEmail = FindUserInfoByEmail(email);
        string answer = "";
        
        answer = findUserByEmail != null 
            ? findUserByEmail.ToString() 
            : $"[!] Cannot find user with this email: {email}.";

        return answer;
    }

    public static void Main()
    {
        //GenerateDataBase();
        Console.WriteLine(GetUserInfoByEmail("email4437@gmail.com"));
    }
}