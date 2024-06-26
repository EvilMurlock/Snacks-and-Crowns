using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGolem : DialogueComponentData<ActivateGolem>
{
    // splite - FACE
    [SerializeField]
    GameObject golemPrefab;
    [SerializeField]
    Dialogue newDialogue;
    public override void InicializeComponent(GameObject player, GameObject listener)
    {
        Debug.Log("Golem Activated");
        GameObject npc = GameObject.Instantiate(golemPrefab, listener.transform.position, listener.transform.rotation);
        npc.GetComponent<FactionMembership>().Faction = listener.GetComponent<FactionMembership>().Faction;
        npc.GetComponent<DialogueManager>().startDialogue = newDialogue;
        GameObject.Destroy(listener.gameObject);
        // give goal FOLLOW PLAYER
        // CHANGE to PLAYER FACTION
        // CHANGE FACE TO ACTIVE FACE
    }
}
