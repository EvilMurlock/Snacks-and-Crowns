using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Manages interaction with objects, remembers long run interactions and highlights objects for interaction
/// </summary>
public class PlayerInteractManager : MonoBehaviour
{
    List<GameObject> interactiblesInRange;
    Interactible interactedObjekt;
    PlayerStateManager playerStateManager;
    Color highlightColor = Color.cyan;
    void Awake()
    {
        interactiblesInRange = new List<GameObject>();
    }
    private void Start()
    {
        playerStateManager = GetComponent<PlayerStateManager>();
    }
    private void Update()
    {
        if (interactiblesInRange.Count > 0)

        {
            if (interactiblesInRange[0] == null) interactiblesInRange.Remove(interactiblesInRange[0]);
            UnHighlight(interactiblesInRange[0]);
            interactiblesInRange.Sort(delegate (GameObject a, GameObject b)
            {
                return Vector2.Distance(this.transform.position, a.transform.position)
                .CompareTo(
                  Vector2.Distance(this.transform.position, b.transform.position));
            });
            Highlight(interactiblesInRange[0]);
        }
    }
    void Highlight(GameObject g)
    {
        if(g != null && g.GetComponent<SpriteRenderer>() != null)
        {
            g.GetComponent<SpriteRenderer>().color = highlightColor;
        }
    }
    void UnHighlight(GameObject g)
    {
        if (g != null && g.GetComponent<SpriteRenderer>() != null)
        {
            g.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(interactiblesInRange.Count > 0)
            {
                GameObject thing = interactiblesInRange[0];
                if (thing == null)
                {
                    interactiblesInRange.RemoveAt(0);
                    Interact(context);
                }

                Interactible interactible = thing.GetComponent<Interactible>();
                interactible.Interact(gameObject);

                if (interactible.LockMove)
                    playerStateManager.ChangeState(CharacterState.inMenu);
                interactedObjekt = interactible;

            }
        }
    }
    public void UnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (interactedObjekt == null) return;
            interactedObjekt.UnInteract(gameObject);
            UnInteract();
        }
    }
    public void UnInteract()
    {
        if (interactedObjekt == null) return;
        if (interactedObjekt.LockMove)
            playerStateManager.ChangeState(CharacterState.normal);
        interactedObjekt = null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Interactible>())
        {
            interactiblesInRange.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        UnHighlight(collision.gameObject);
        interactiblesInRange.Remove(collision.gameObject);
    }
}
