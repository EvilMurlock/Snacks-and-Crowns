using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand_Item_Controler : MonoBehaviour
{ 
    Animator animator;
    public AnimationClip idle;
    public AnimationClip use;
    public List<Attack> attacks;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(idle.name);
    }
    public void Use()
    {
        animator.Play(use.name);
    }
    public void Go_Idle()
    {
        animator.Play(idle.name);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Damagable damagable = collision.gameObject.GetComponent<Damagable>();
        if (damagable != null)
        {
            foreach(Attack attack in attacks)
            {
                damagable.Take_Damage(attack);
            }
        }
        //Debug.Log("Colision with: " + collision.gameObject.name);
        //FIRE EFFECTS ON OBJECTS THAT ARE RELEVANT
        //collision.gameObject.GetComponent<>();//GET THE DAMAGABLE COMPONENT    

    }
}
