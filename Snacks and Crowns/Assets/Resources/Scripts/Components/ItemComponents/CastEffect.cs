using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Generates object that provides particle effects, then removes this object after item is done being used
/// </summary>
public class CastEffect : ItemComponentData<CastEffect>
{
    //[HideInInspector]
    public GameObject effect;
    public CastEffect()
    {
        activateAtUse = true;
    }
    public override void InicializeComponent(GameObject user, Item item)
    {
        
        GameObject effectObject = GameObject.Instantiate(effect, user.transform, false);
        DeleteAfterTime delete = effectObject.AddComponent<DeleteAfterTime>();
        delete.Init(item.useDuration);
        
    }
}
