using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleDoor : Interactible_Object
{
    bool open = false;
    public override void Interact(GameObject new_player)
    {
        if (open)
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z+90);
            open = false;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z - 90);
            open = true;
        }
    }
}
