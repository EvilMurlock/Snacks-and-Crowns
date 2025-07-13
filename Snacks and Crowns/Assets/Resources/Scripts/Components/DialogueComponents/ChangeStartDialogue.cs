using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

/// <summary>
/// Changes the start dialogue of a DialogueManager
/// </summary>
public class ChangeStartDialogue : DialogueComponentData<ActivateGolem>
{
    [SerializeField]
    Dialogue newDialogue;
    public override void InitializeComponent(GameObject player, GameObject listener)
    {
        listener.GetComponent<DialogueManager>().startDialogue = newDialogue;
    }
}
