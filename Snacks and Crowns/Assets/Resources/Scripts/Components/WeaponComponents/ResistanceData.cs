using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Data for a Resistance component
/// </summary>
[System.Serializable]
public class ResistanceData : EquipmentComponentData<ResistanceData>
{
    public DamageType resistanceType;
    public float resistanceValue;
    public override void InicializeComponent(GameObject equipmentObject, Item item)
    {

        Resistance res = equipmentObject.AddComponent<Resistance>();
        res.resistanceType = resistanceType;
        res.resistanceValue = resistanceValue;
        res.AddResistance(equipmentObject.transform.parent.parent.gameObject);

    }
}
