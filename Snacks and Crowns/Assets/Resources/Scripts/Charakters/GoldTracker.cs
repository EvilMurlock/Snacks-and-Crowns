using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GoldTracker : MonoBehaviour
{
    int gold = 100;
    public UnityEvent<int> goldChange;
    public int Gold { get { return gold; } }
    public void AddGold(int difference)
    {
        gold += difference;
        goldChange.Invoke(gold);
    }
    public bool HasGold(int atleastGold)
    {
        return gold >= atleastGold;
    }
}
