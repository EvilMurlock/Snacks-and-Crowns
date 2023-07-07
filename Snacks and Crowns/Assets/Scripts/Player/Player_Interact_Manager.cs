using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player_Interact_Manager : MonoBehaviour
{
    List<GameObject> interactibles_in_range;
    GameObject interactedObjekt;

    void Awake()
    {
        interactibles_in_range = new List<GameObject>();
    }
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(interactibles_in_range.Count > 0)
            {
                GameObject thing = interactibles_in_range[0];
                if (thing == null)
                {
                    interactibles_in_range.RemoveAt(0);
                    Interact(context);
                }
                if (thing.GetComponent<Item_Controler>())
                {
                    GetComponent<Player_Inventory>().Pick_Up_Item(thing);
                }
                if (thing.GetComponent<Interactible_Object>())
                {
                    thing.GetComponent<Interactible_Object>().Interact(gameObject);
                    interactedObjekt = thing;
                }
                if (thing.GetComponent<DialogueManager>())
                {
                    thing.GetComponent<DialogueManager>().StartDialogue(gameObject);
                    interactedObjekt = thing;
                }

            }
        }
    }
    public void UnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (interactibles_in_range.Count > 0)
            {
                GameObject thing = interactedObjekt;
                if (thing.GetComponent<Interactible_Object>())
                {
                    thing.GetComponent<Interactible_Object>().Un_Interact(gameObject);
                    interactedObjekt = thing;
                }
                if (thing.GetComponent<DialogueManager>())
                {
                    thing.GetComponent<DialogueManager>().EndDialogue();
                    interactedObjekt = thing;
                }
                gameObject.GetComponent<Player_State_Manager>().Change_State(Player_State.normal);
            }
        }

    }
    void Update()
    {
        interactibles_in_range.Sort(delegate (GameObject a, GameObject b)
        {
            return Vector2.Distance(this.transform.position, a.transform.position)
            .CompareTo(
              Vector2.Distance(this.transform.position, b.transform.position));
        });
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Item_Controler>())
        {
            interactibles_in_range.Add(collision.gameObject);
        }
        if (collision.GetComponent<Interactible_Object>())
        {
            interactibles_in_range.Add(collision.gameObject);
        }
        if (collision.GetComponent<DialogueManager>())
        {
            interactibles_in_range.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        interactibles_in_range.Remove(collision.gameObject);
    }
}
