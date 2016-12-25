using UnityEngine;
using System.Collections.Generic; 
using System.Linq;

public class SRand  {
    static int[] _values; 
    public static int GetStartingIndex()
    {
        return Random.Range(0, _values.Length); 
    }
    public static int Roll(ref int index)
    {
        int _value = _values[index];
        index = (index + 1) % _values.Length;
        Debug.Log("Roll: " + index + " | "  +_value); 
        return _value;
    }
    public static void ReverseRoll(ref int index)
    {
        index--; 
        if(index < 0)
        {
            index = _values.Length - 1;  
        }
    }
    public static void Awake()
    {
        IEnumerable<int> _oneSet = Enumerable.Range(0, 100);
        _values = _oneSet.Concat(_oneSet).Concat(_oneSet).Concat(_oneSet).ToArray<int>(); 
        for(int i = 0; i < _values.Length; i++)
        {
            int _newIndex = Random.RandomRange(0, _values.Length);
            int _tempValue = _values[_newIndex];
            _values[_newIndex] = _values[i];
            _values[i] = _tempValue; 
        }
    }
}
