using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Automatically opens and closes adjacent doors
/// </summary>
public class DoorOpener : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Door door;
        if (collision.gameObject.TryGetComponent<Door>(out door))
        {
            if (!door.open) door.Interact(this.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Door door;
        if(collision.gameObject.TryGetComponent<Door>(out door))
        {
            if (!door.open) door.Interact(this.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Door door;
        if (collision.gameObject.TryGetComponent<Door>(out door))
        {
            if (door.open)door.Interact(this.gameObject);
        }
    }
}
