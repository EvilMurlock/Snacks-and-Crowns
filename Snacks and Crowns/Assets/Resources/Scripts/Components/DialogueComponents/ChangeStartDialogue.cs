using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class ChangeStartDialogue : DialogueComponentData<ActivateGolem>
{
    [SerializeField]
    Dialogue newDialogue;
    public override void InicializeComponent(GameObject player, GameObject listener)
    {
        listener.GetComponent<DialogueManager>().startDialogue = newDialogue;
    }
}
