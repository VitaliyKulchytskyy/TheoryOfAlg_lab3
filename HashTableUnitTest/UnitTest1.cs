namespace TheoryOfAlg_lab3;

public class TestAddRemove
{
    [Test]
    public void CheckAddValue()
    {
        var testTable = new HashTable<Key, string>()
        {
            [new Key("aaa", 0)] = "i",
            [new Key("aaa", 1)] = "me",
            [new Key("aaa", 2)] = "mine",
            [new Key("aaa", 5)] = "over",
            [new Key("aaa", 18)] = "rebel",
            [new Key("aaa", 6)] = "lady black"
        };
        testTable[new Key("aaa", 11)] = "apple";
        testTable[new Key("aaa", 12)] = "break free";
        
        Assert.AreEqual(8, testTable.Size);
    }
    
    [Test]
    public void CheckRemoveRealValues()
    {
        var testTable = new HashTable<Key, string>()
        {
            [new Key("aaa", 0)] = "i",
            [new Key("aaa", 1)] = "me",
            [new Key("aaa", 2)] = "mine",
            [new Key("aaa", 5)] = "over",
            [new Key("aaa", 18)] = "rebel",
            [new Key("aaa", 6)] = "lady black"
        };
        testTable.Remove(new Key("aaa", 18));
        testTable.Remove(new Key("aaa", 1));
        
        Assert.AreEqual(4, testTable.Size);
    }
    
    [Test]
    public void CheckRemoveNotExistValues()
    {
        var testTable = new HashTable<Key, string>()
        {
            [new Key("aaa", 0)] = "i",
            [new Key("aaa", 1)] = "me",
            [new Key("aaa", 2)] = "mine",
            [new Key("aaa", 5)] = "over",
            [new Key("aaa", 18)] = "rebel",
            [new Key("aaa", 6)] = "lady black"
        };
        //testTable.Remove(new Key("aaa", 118));
        testTable.Remove(new Key("aaa", 111));
        
        Assert.AreEqual(6, testTable.Size);
    }
}

public class TestKey
{
    [Test]
    public void CheckGetRealValueByKey()
    {
        var testTable = new HashTable<Key, string>()
        {
            [new Key("aaa", 0)] = "i",
            [new Key("aaa", 1)] = "me",
            [new Key("aaa", 2)] = "mine",
            [new Key("aaa", 5)] = "over",
            [new Key("aaa", 18)] = "rebel",
            [new Key("aaa", 6)] = "lady black"
        };

        Assert.AreEqual("over", testTable[new Key("aaa", 5)]);
    }
    
    [Test]
    public void CheckGetNotExistValueByKey()
    {
        var testTable = new HashTable<Key, string>()
        {
            [new Key("aaa", 0)] = "i",
            [new Key("aaa", 1)] = "me",
            [new Key("aaa", 2)] = "mine",
            [new Key("aaa", 5)] = "over",
            [new Key("aaa", 18)] = "rebel",
            [new Key("aaa", 6)] = "lady black"
        };

        Assert.AreEqual(default(string), testTable[new Key("aaa", 151)]);
    }

    [Test]
    public void CheckIsContainsKey()
    {
        var testTable = new HashTable<Key, string>()
        {
            [new Key("aaa", 0)] = "i",
            [new Key("aaa", 1)] = "me",
            [new Key("aaa", 2)] = "mine",
            [new Key("aaa", 5)] = "over",
            [new Key("aaa", 18)] = "rebel",
            [new Key("aaa", 6)] = "lady black"
        };

        Assert.True(testTable.ContainsKey(new Key("aaa", 18)));
    }

    [Test]
    public void CheckIsNotContainsKey()
    {
        var testTable = new HashTable<Key, string>()
        {
            [new Key("aaa", 0)] = "i",
            [new Key("aaa", 1)] = "me",
            [new Key("aaa", 2)] = "mine",
            [new Key("aaa", 5)] = "over",
            [new Key("aaa", 18)] = "rebel",
            [new Key("aaa", 6)] = "lady black"
        };

        Assert.False(testTable.ContainsKey(new Key("aaa", 17)));
    }
}

public class TestLoadFactory
{
    [Test]
    public void CheckNotIncreasedCapacity()
    {
        var testTable = new HashTable<Key, string>(4)
        {
            [new Key("aaa", 0)] = "i",
            [new Key("aaa", 1)] = "me"
        };

        Assert.AreEqual(4, testTable.Capacity);
    }
    
    [Test]
    public void CheckIncreasedCapacity()
    {
        var testTable = new HashTable<Key, string>(4)
        {
            [new Key("aaa", 0)] = "i",
            [new Key("aaa", 1)] = "me",
            [new Key("aaa", 2)] = "mine",
            [new Key("aaa", 5)] = "over"
        };

        Assert.AreEqual(8, testTable.Capacity);
    }
    
    [Test]
    public void CheckIsHashWasRecalculated()
    {
        Key checkKey = new Key("aaa", 7);
        var a = new HashTable<Key, string>(4)
        {
            [new Key("aaa", 3)] = "i", // expects collision with next
            [checkKey] = "me" // expects collision with previous
        };
        
        var b = new HashTable<Key, string>(4)
        {
            [new Key("aaa", 3)] = "i", // doesn't expect collision with next
            [checkKey] = "me", // doesn't expect collision with previous
            [new Key("aaa", 0)] = "mine",
            [new Key("aaa", 1)] = "over"
        };

        Assert.AreNotEqual(a.CalcIndexByKey(checkKey), b.CalcIndexByKey(checkKey));
    }
}