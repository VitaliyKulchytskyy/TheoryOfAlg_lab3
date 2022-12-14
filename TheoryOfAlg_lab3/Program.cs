using System.Collections;
using Microsoft.Win32.SafeHandles;
using TheoryOfAlg_lab3.UserData;

namespace TheoryOfAlg_lab3;

public static class Program
{
    private const string PathDB = "/home/karlenko/projects/csharp/TheoryOfAlg_lab3/TheoryOfAlg_lab3/UserData/UsersDataBase.db";
    private const string PathMeta = "/home/karlenko/projects/csharp/TheoryOfAlg_lab3/TheoryOfAlg_lab3/UserData/MetadataOfDB.db";
    private const int UserAmount = 1000;
    
    static void GenerateDataBase(int amountOfUser = UserAmount, string path = PathDB)
    {
        var hashTable = new HashTable<StringHash, User>(UserAmount, false);
        var hashMetaTable = new HashTable<ushort, Metadata>(UserAmount, false);

        ulong offset = 0;
        for (int i = 0; i < amountOfUser; i++)
        {
            StringHash genEmail = new StringHash("email" + i + "@gmail.com");
            hashTable[genEmail] = new User("User" + i , genEmail.ToString(), (byte) (1 + i % 87));
        }

        int counter = 0;
        foreach (var elem in hashTable)
        {
            ushort index = (ushort)hashTable.CalcIndexByKey(new StringHash(elem.Email));
            hashMetaTable[index] = new Metadata(index, offset);

            hashMetaTable[(ushort)counter] = new Metadata((ushort)counter, 0);
            if (!hashMetaTable.ContainsKey(index))
            {
                hashMetaTable[(ushort)counter] = new Metadata((ushort)counter, 0);
            }
            
            offset += User.SerializedBytesPerUser;
            counter++;
        }
        
        //hashMetaTable.PrintBuckets();
        //hashTable.PrintBuckets();

        using (var file = File.OpenWrite(path))
        {
            foreach (var elem in hashTable)
            {
                file.Write(elem.Serialize());
            }
        }

        using (var fileMeta = File.OpenWrite(PathMeta))
        {
            foreach (var meta in hashMetaTable)
            {
                fileMeta.Write(meta.Serialize());
            }
        }

        Console.WriteLine("[?] DataBase successfully generated.\n");
    }

    static ulong GetHashValueOffset(int hashValue)
    {
        using (var file = new BinaryReader(File.OpenRead(PathMeta)))
        {
            file.BaseStream.Seek(hashValue * Metadata.MetaBlockBytes, SeekOrigin.Begin);
         
            byte[] buffer = new byte[Metadata.MetaBlockBytes];
            file.Read(buffer);
            return Metadata.Deserialize(buffer).Offset;
        }
    }
    
    static User RandomAccessFromDataBase(int hashValue, int userIndex = 0, string path = PathDB)
    {
        using (var file = new BinaryReader(File.OpenRead(path)))
        {
            file.BaseStream.Seek((long)GetHashValueOffset(hashValue) + userIndex * User.SerializedBytesPerUser, SeekOrigin.Begin);
         
            byte[] buffer = new byte[User.SerializedBytesPerUser];
            file.Read(buffer);
            return User.Deserialize(buffer);
        }
    }

    static User? FindUserInfoByEmail(string email)
    {
        StringHash emailHash = new StringHash(email);
        var hashTable = new HashTable<StringHash, User>(UserAmount, false);
        int hashValue = hashTable.CalcIndexByKey(emailHash);
        int index = 0;
        User output = null;

        do
        {
            output = RandomAccessFromDataBase(hashValue, index++);
            if (index > UserAmount)
                return null;
        } while (!emailHash.Equals(output.Email)); 

        return output;
    }

    static string GetUserInfoByEmail(string email, ref int successCount)
    {
        User? findUserByEmail = FindUserInfoByEmail(email);
        string answer = "";

        if (findUserByEmail != null)
        {
            answer = findUserByEmail.ToString();
            successCount++;
        }
        else
        {
            answer = $"[!] Cannot find user with this email: {email}.";
        }

        return answer;
    }

    public static void Main()
    {
        GenerateDataBase();
        
        int successCount = 0;
        for (int i = 0; i < UserAmount; i++)
        {
            GetUserInfoByEmail($"email{i}@gmail.com", ref successCount);
        }
        Console.WriteLine($"{successCount} / {UserAmount}");
    }
}