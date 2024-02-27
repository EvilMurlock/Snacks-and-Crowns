using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Door door = collision.gameObject.GetComponent<Door>();
        if (collision.gameObject.GetComponent<Door>() != null)
        {
            //Debug.Log("Door entered range");
            if(!door.open) door.Interact(this.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Door door = collision.gameObject.GetComponent<Door>();
        if (collision.gameObject.GetComponent<Door>() != null)
        {
            //Debug.Log("Door entered range");
            if (!door.open) door.Interact(this.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Door door = collision.gameObject.GetComponent<Door>();
        if (collision.gameObject.GetComponent<Door>() != null)
        {
            if(door.open)door.Interact(this.gameObject);
        }
    }
}
