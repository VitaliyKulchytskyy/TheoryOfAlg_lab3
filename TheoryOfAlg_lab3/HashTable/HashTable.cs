using System.Collections;

namespace TheoryOfAlg_lab3;

public class HashTable<TKey, TValue>: IEnumerable<TValue>
{
    public  uint Capacity { get; private set; }
    public uint Size { get; private set; }
    private const float LoadFactory = 0.75f;
    private readonly bool _isDynamicTable = true;
    private Node<TKey, TValue>?[] _storage;

    public HashTable(uint capacity = 16, bool isDynamicTable = true)
    {
        Capacity = capacity;
        _isDynamicTable = isDynamicTable;
        _storage = new Node<TKey, TValue>[Capacity];
    }

    public HashTable(HashTable<TKey, TValue> other)
    {
        Capacity = other.IsNeedRehash() ? other.Capacity * 2 : other.Capacity;
        Size = other.Size;
        _storage = new Node<TKey, TValue>[Capacity];
     
        foreach (var node in other._storage)
        {
            if (node != null)
            {
                int index = other.CalcIndexByKey(node.GetKey);
                for (Node<TKey, TValue>? it = other._storage[index]; it != null; it = it.Next)
                    this[it.GetKey] = it.Value;
            }
        }
    }

    public TValue? this[TKey key]
    {
        get => GetValueByKey(key);
        set => AddValue(key, value);
    }

    public bool ContainsKey(TKey key)
    {
        int index = CalcIndexByKey(key);
        if (_storage[index] == null)
            return false;

        for(Node<TKey, TValue> node = _storage[index]; node != null; node = node.Next)
            if (node.GetKey.Equals(key))
                return true;
        
        return false;
    }

    public void Remove(TKey key)
    {
        if (!ContainsKey(key))
            return;
        
        Size--;
        
        int index = CalcIndexByKey(key);
        Node<TKey, TValue> temp = _storage[index];
        if (temp.GetKey.Equals(key))
        {
            _storage[index] = temp.Next;
            return;
        }

        Node<TKey, TValue> curr = _storage[index];
        while (temp != null && !temp.GetKey.Equals(key))
        {
            curr = temp;
            temp = temp.Next;
        }

        curr.Next = temp.Next;
    }

    private void AddValue(TKey key, TValue value)
    {
        if (IsNeedRehash())
            Rehash();

        Size++;
        
        int index = CalcIndexByKey(key);
        if (_storage[index] == null)
        {
            _storage[index] = new Node<TKey, TValue>(key, value, null);
            return;
        }

        Node<TKey, TValue>? lastNode = null;
        for (Node<TKey, TValue>? checker = _storage[index]; checker != null; checker = checker.Next)
        {
           if (checker.GetKey.Equals(key)) 
               return;
           if (checker.Next == null)
               lastNode = checker;
        }

        lastNode!.Next = new Node<TKey, TValue>(key, value, null);
    }

    public void PrintBuckets()
    {
        foreach (var node in _storage)
        {
            if (node != null)
            {
                int index = CalcIndexByKey(node.GetKey);
                for (Node<TKey, TValue>? it = _storage[index]; it != null; it = it.Next)
                    Console.WriteLine($"[{CalcIndexByKey(it.GetKey)} ({it.GetKey.GetHashCode()})]: {it.Value}");
            }
        }
        Console.WriteLine($"| Size: {Size} | Capacity: {Capacity} |");
    }

    public int CalcIndexByKey(TKey key)
        => (int)Math.Abs(key.GetHashCode() % Capacity);

    private TValue? GetValueByKey(TKey key)
    {
        int index = CalcIndexByKey(key);

        for (Node<TKey, TValue>? node = _storage[index]; node != null; node = node.Next)
            if (node.GetKey.Equals(key))
                return node.Value;

        return default;
    }

    private bool IsNeedRehash()
        => _isDynamicTable && ((100d * Size) / Capacity) / 100d >= LoadFactory;

    private void Rehash()
    {
        var temp = new HashTable<TKey, TValue>(this);
        Size = temp.Size;
        _storage = temp._storage;
        Capacity = temp.Capacity;
    }

    private Node<TKey, TValue>? FindNextAccessibleNode(int fromIndex)
    {
        if (fromIndex == Capacity)
            return null;
        
        Node<TKey, TValue> output;
        for (output = _storage[fromIndex]; output == null; fromIndex++)
        {
            if (fromIndex >= Capacity)
                return null;
            output = _storage[fromIndex];
        }

        return output;
    }
    
    //================================

    public IEnumerator<TValue> GetEnumerator() => new HashEnumerator(this);

    private IEnumerator GetEnumerator1() => GetEnumerator();
    
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator1();
    
    class HashEnumerator: IEnumerator<TValue>
    {
        private Node<TKey, TValue>? _currNode = null;
        private HashTable<TKey, TValue> _parent;

        public HashEnumerator(HashTable<TKey, TValue> parent)
        {
            _parent = parent;
        }

        public bool MoveNext()
        {
            if (_currNode == null)
            {
                _currNode = _parent.FindNextAccessibleNode(0);
                return _currNode != null;
            }

            if (_currNode != null)
            {
                if (_currNode.Next == null)
                {
                    _currNode = _parent.FindNextAccessibleNode(_parent.CalcIndexByKey(_currNode.GetKey) + 1);
                    return _currNode != null;
                }
          
                _currNode = _currNode.Next;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            _currNode = null;
        }

        object IEnumerator.Current => Current;

        public TValue Current => _currNode!.Value!;
        
        public void Dispose()
        { }
    }
}