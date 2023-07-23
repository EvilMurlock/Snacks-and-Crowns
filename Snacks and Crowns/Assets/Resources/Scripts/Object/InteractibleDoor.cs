using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleDoor : Interactible_Object
{
    public bool open = false;
    public AnimationClip closeAnim;
    public AnimationClip openAnim;
    public Animator animator;
    public void Start()
    {
        animator = transform.parent.gameObject.GetComponent<Animator>();
        animator.Play(closeAnim.name);
    }
    public override void Interact(GameObject new_player)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(closeAnim.name)
            || !animator.GetCurrentAnimatorStateInfo(0).IsName(openAnim.name))
        {
            if (open)
            {
                animator.Play(closeAnim.name);
                //StartCoroutine(ChangeAnimation(closingAnim.length, closedAnim));
                open = false;
            }
            else
            {
                animator.Play(openAnim.name);
                //StartCoroutine(ChangeAnimation(openingAnim.length, openAnim));
                open = true;
            }
        }
        else ;

    }
    IEnumerator ChangeAnimation(float duration, AnimationClip new_animation)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log(new_animation.name);
        animator.Play(new_animation.name);
    }
}
