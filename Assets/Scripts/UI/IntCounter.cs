using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

public class IntCounter : ScriptableObject
{
    private int _count;
    public int Count => _count;
    public event Action<int> CountChanged = delegate { };

    public void IncrementCount()
    {
        _count++;
        CountChanged.Invoke(_count);
    }

    public void ResetCount()
    {
        _count = 0;
        CountChanged.Invoke(_count);
    }
}
