using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Manages instances of hand items
/// </summary>
public class HandItemController : MonoBehaviour
{ 
    Animator animator;
    public AnimationClip idle;
    public AnimationClip use;
    List<GameObject> collidedObjects = new List<GameObject>();
    public UnityEvent<GameObject> onHitEvent;
    public UnityEvent<GameObject> spriteChangeEvent;
    bool beingUsed = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(idle.name);
    }
    public void Use()
    {
        try
        {
            if (!beingUsed)
            {
                collidedObjects.Clear();
                if (use == null)
                {
                    Debug.Log("ANIMATION IS NULLL => " + gameObject.name + " | " + transform.position);
                }
                animator.Play(use.name);
                beingUsed = true;
            }
        }
        catch
        {
            Debug.Log("YEP ERRORING HERE");
        }
    }
    public void Go_Idle()
    {
        animator.Play(idle.name);
        beingUsed = false;
    }
    public void SpriteChangeInAnimation()
    {
        spriteChangeEvent.Invoke(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject colObject = collision.gameObject;
        //Isn't the wielder, can be hit and has not yet been hit
        if (colObject! != transform.parent.parent.gameObject && 
            !collidedObjects.Contains(colObject) &&
            !collision.isTrigger)
        {
            collidedObjects.Add(colObject);
            onHitEvent.Invoke(colObject);
        }

    }
}
