using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resistance : MonoBehaviour
{
    public Damage_Type resistanceType;
    public float resistanceValue;
    GameObject resistingObject;
    public void AddResistance(GameObject resistingObject)
    {
        this.resistingObject = resistingObject;
        if (this.gameObject.GetComponent<onDestroy>() == null)
        {
            this.gameObject.AddComponent<onDestroy>();
        }
        Debug.Log("Exists?: " + this.gameObject.GetComponent<onDestroy>().destroyEvent);
        this.gameObject.GetComponent<onDestroy>().destroyEvent.AddListener(RemoveResistance);
        resistingObject.GetComponent<Damagable>().ChangeResistance(resistanceType, resistanceValue);
    }
    public void RemoveResistance()
    {
        resistingObject.GetComponent<Damagable>().ChangeResistance(resistanceType, -resistanceValue);
    }
}
