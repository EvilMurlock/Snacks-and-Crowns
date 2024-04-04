using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    public virtual bool LockMove { get { return false; } }
    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponent<FactionMembership>() == null) gameObject.AddComponent<FactionMembership>();
    }
    public abstract void Interact(GameObject player);
    public virtual void UnInteract(GameObject player) { throw new System.Exception("UnInteract method not implemented in object: " + this.name); }
}
