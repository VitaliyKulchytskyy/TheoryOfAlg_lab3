using System.Collections;

namespace TheoryOfAlg_lab3;

public class Node<TKey, TValue>
{
    public TValue Value;
    public Node<TKey, TValue>? Next;
    public readonly TKey GetKey;

    public Node(TKey key, TValue value, Node<TKey, TValue>? next)
    {
        GetKey = key;
        Value = value;
        Next = next;
    }
}