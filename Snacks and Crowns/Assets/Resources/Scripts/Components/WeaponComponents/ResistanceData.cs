using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceData : EquipmentComponentData<ResistanceData>
{
    public Damage_Type resistanceType;
    public float resistanceValue;
    public override void InicializeComponent(GameObject equipmentObject, Item item)
    {

        Resistance res = equipmentObject.AddComponent<Resistance>();
        res.resistanceType = resistanceType;
        res.resistanceValue = resistanceValue;
        res.AddResistance(equipmentObject.transform.parent.parent.gameObject);

    }
}
