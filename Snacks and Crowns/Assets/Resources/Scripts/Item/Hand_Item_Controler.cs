using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Hand_Item_Controler : MonoBehaviour
{ 
    Animator animator;
    public AnimationClip idle;
    public AnimationClip use;
    List<GameObject> colidedObjects;
    public UnityEvent<GameObject> onHitEvent;
    public UnityEvent<GameObject> spriteChangeEvent;

    // Start is called before the first frame update
    void Start()
    {
        colidedObjects = new List<GameObject>();
        animator = GetComponent<Animator>();
        animator.Play(idle.name);
    }
    public void Use()
    {
        colidedObjects.Clear();
        animator.Play(use.name);
    }
    public void Go_Idle()
    {
        animator.Play(idle.name);
    }
    public void SpriteChangeInAnimation()
    {
        spriteChangeEvent.Invoke(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject colObject = collision.gameObject;
        //Isnt the weilder, can be hit and has not yet been hit
        if (colObject! != transform.parent.parent.gameObject && !colidedObjects.Contains(colObject))
        {
            colidedObjects.Add(colObject);
            onHitEvent.Invoke(colObject);
        }
        //Debug.Log("Colision with: " + collision.gameObject.name);
        //FIRE EFFECTS ON OBJECTS THAT ARE RELEVANT
        //collision.gameObject.GetComponent<>();//GET THE DAMAGABLE COMPONENT    

    }
}
