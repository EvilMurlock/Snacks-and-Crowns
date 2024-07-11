using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class ActivateGolem : DialogueComponentData<ActivateGolem>
{
    [SerializeField]
    GameObject golemPrefab;
    [SerializeField]
    Dialogue newDialogue;
    public override void InicializeComponent(GameObject player, GameObject listener)
    {
        Debug.Log("Golem Activated");
        GameObject npc = GameObject.Instantiate(golemPrefab, listener.transform.position, listener.transform.rotation);
        npc.GetComponent<FactionMembership>().Faction = player.GetComponent<FactionMembership>().Faction;
        GameObject.Destroy(listener.gameObject);

        npc.GetComponent<DialogueManager>().startDialogue = newDialogue;

        npc.AddComponent<GolemFightGoal>();

        var goalGoTo = npc.AddComponent<GoToLocation>();
        goalGoTo.SetDesiredTarget(player);
        goalGoTo.enabledGoal = false;

        npc.AddComponent<IdleAroundAPoint>();

        var goalRaid = npc.AddComponent<LaunchRaid>();
        goalRaid.enabledGoal = false;

        var refresher = npc.AddComponent<AstarGridRefresher>();
        GameObject.Destroy(refresher, 5);
    }
}
