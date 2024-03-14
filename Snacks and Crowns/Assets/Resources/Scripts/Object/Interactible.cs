using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    [SerializeField]
    bool lockMove;
    public bool LockMove { get { return lockMove; } }
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
