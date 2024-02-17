using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GoldTracker : MonoBehaviour
{
    int gold;
    UnityEvent<int> goldChange;
    public int Gold { get { return gold; } }
    public void AddGold(int difference)
    {
        gold += difference;
        goldChange.Invoke(gold);
    }
}
