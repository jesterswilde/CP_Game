using UnityEngine;
using System.Collections;

public struct PossibleFuture {
    HistoryNode _head;
    HistoryNode _tail; 
    public PossibleFuture(HistoryNode head, HistoryNode tail)
    {
        _head = head;
        _tail = tail; 
    }
    public HistoryNode Tail { get { return _tail; } }
    public HistoryNode Head { get { return _head; } }
    public static PossibleFuture Null { get { return new PossibleFuture(null, null); } }
}
