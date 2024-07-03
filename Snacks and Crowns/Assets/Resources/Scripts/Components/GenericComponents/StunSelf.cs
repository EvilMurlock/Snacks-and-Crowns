using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StunSelf : ComponentDataGeneric, IGenericComponent
{
    [SerializeField]
    float stunDuration;
    public override void InicializeComponent(GameObject self)
    {
        if (self.GetComponent<Movement>() != null) self.GetComponent<Movement>().Stun(stunDuration);
    }
}
