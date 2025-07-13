using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stuns this character
/// </summary>
public class StunSelf : ComponentDataGeneric, IGenericComponent
{
    [SerializeField]
    float stunDuration;
    public override void InitializeComponent(GameObject self)
    {
        if (self.GetComponent<Movement>() != null) self.GetComponent<Movement>().Stun(stunDuration);
    }
}
