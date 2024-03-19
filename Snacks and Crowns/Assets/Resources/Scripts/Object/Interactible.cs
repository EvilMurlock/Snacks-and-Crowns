using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    public abstract bool LockMove { get; }
    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponent<FactionMembership>() == null) gameObject.AddComponent<FactionMembership>();
    }
    public virtual void Interact(GameObject player)
    {

    }
    public virtual void UnInteract(GameObject player)
    {

    }

}
