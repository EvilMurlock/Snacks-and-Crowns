using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        InteractibleDoor door = collision.gameObject.GetComponent<InteractibleDoor>();
        if (collision.gameObject.GetComponent<InteractibleDoor>() != null)
        {
            //Debug.Log("Door entered range");
            if(!door.open) door.Interact(this.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        InteractibleDoor door = collision.gameObject.GetComponent<InteractibleDoor>();
        if (collision.gameObject.GetComponent<InteractibleDoor>() != null)
        {
            //Debug.Log("Door entered range");
            if (!door.open) door.Interact(this.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        InteractibleDoor door = collision.gameObject.GetComponent<InteractibleDoor>();
        if (collision.gameObject.GetComponent<InteractibleDoor>() != null)
        {
            if(door.open)door.Interact(this.gameObject);
        }
    }
}
