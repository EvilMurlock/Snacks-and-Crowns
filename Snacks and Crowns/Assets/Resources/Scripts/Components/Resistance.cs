using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Adds resistance on initialization, removes resistance on deletion
/// </summary>
public class Resistance : MonoBehaviour
{
    public DamageType resistanceType;
    public float resistanceValue;
    GameObject resistingObject;
    public void AddResistance(GameObject resistingObject)
    {
        this.resistingObject = resistingObject;
        if (this.gameObject.GetComponent<onDestroy>() == null)
        {
            this.gameObject.AddComponent<onDestroy>();
        }
        this.gameObject.GetComponent<onDestroy>().destroyEvent.AddListener(RemoveResistance);
        resistingObject.GetComponent<Damageable>().ChangeResistance(resistanceType, resistanceValue);
    }
    public void RemoveResistance()
    {
        resistingObject.GetComponent<Damageable>().ChangeResistance(resistanceType, -resistanceValue);
    }
}
