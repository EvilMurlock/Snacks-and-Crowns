using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGolem : DialogueComponentData<ActivateGolem>
{
   // splite - FACE
    public override void InicializeComponent(GameObject player, GameObject listener)
    {
        Debug.Log("Golem Activated");
        // give goal FOLLOW PLAYER
        // CHANGE to PLAYER FACTION
        // CHANGE FACE TO ACTIVE FACE
    }
}
