using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject objectsSon = transform.Find("Objects").gameObject;
        foreach(Transform childTransform in objectsSon.transform)
        {
            GameObject child = childTransform.gameObject;
            if (child.GetComponent<FactionMembership>() == null)
                child.gameObject.AddComponent<FactionMembership>();
            Factions myFaction = this.GetComponent<FactionMembership>().Faction;
            child.GetComponent<FactionMembership>().Faction = myFaction;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}