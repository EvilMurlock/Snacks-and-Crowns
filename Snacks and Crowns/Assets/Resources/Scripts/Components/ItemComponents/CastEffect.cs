using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
