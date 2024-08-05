using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Hand_Item_Controler : MonoBehaviour
{ 
    Animator animator;
    public AnimationClip idle;
    public AnimationClip use;
    List<GameObject> colidedObjects = new List<GameObject>();
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
                colidedObjects.Clear();
                //Debug.Log("Trying to play: " + use.name);
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
        //Debug.Log("Going idle");
    }
    public void SpriteChangeInAnimation()
    {
        spriteChangeEvent.Invoke(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collising with:" + collision.gameObject.name + " | triger: " + collision.isTrigger);
        GameObject colObject = collision.gameObject;
        //Isnt the weilder, can be hit and has not yet been hit
        if (colObject! != transform.parent.parent.gameObject && 
            !colidedObjects.Contains(colObject) &&
            !collision.isTrigger)
        {
            colidedObjects.Add(colObject);
            onHitEvent.Invoke(colObject);
        }
        //Debug.Log("Colision with: " + collision.gameObject.name);
        //FIRE EFFECTS ON OBJECTS THAT ARE RELEVANT
        //collision.gameObject.GetComponent<>();//GET THE DAMAGABLE COMPONENT    

    }
}
