using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class InteractibleDoor : Interactible_Object
{
    public bool open = false;
    public AnimationClip closeAnim;
    public AnimationClip openAnim;
    public Animator animator;
    bool playigAnim;
    public void Start()
    {
        animator = transform.parent.gameObject.GetComponent<Animator>();
        animator.Play(closeAnim.name);
        playigAnim = false;
    }
    public override void Interact(GameObject new_player)
    {
        if (!playigAnim && (!animator.GetCurrentAnimatorStateInfo(0).IsName(closeAnim.name)
            || !animator.GetCurrentAnimatorStateInfo(0).IsName(openAnim.name)))
        {
            if (open)
            {
                animator.Play(closeAnim.name);
                playigAnim = true;
                //StartCoroutine(ChangeAnimation(closingAnim.length, closedAnim));
                open = false;
                StartCoroutine(ChangePathfinding(closeAnim.length, "Default"));
            }
            else
            {
                animator.Play(openAnim.name);
                playigAnim = true;
                //StartCoroutine(ChangeAnimation(openingAnim.length, openAnim));
                StartCoroutine(ChangePathfinding(closeAnim.length,"Obstacle"));

                open = true;
            }
        }
    }
    IEnumerator ChangePathfinding(float time, string newLayer)
    {

        this.gameObject.layer = LayerMask.NameToLayer("Default");
        var guo = new GraphUpdateObject(GetComponentInChildren<BoxCollider2D>().bounds);
        // Set some settings
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);
        yield return new WaitForSeconds(time);
        this.gameObject.layer = LayerMask.NameToLayer(newLayer);
        var guo2 = new GraphUpdateObject(GetComponentInChildren<BoxCollider2D>().bounds);
        // Set some settings
        guo2.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo2);
        playigAnim = false;
    }
    IEnumerator ChangeAnimation(float duration, AnimationClip new_animation)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log(new_animation.name);
        animator.Play(new_animation.name);

    }
}
