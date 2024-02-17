using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuItemSlot : MonoBehaviour
{
    public GameObject button;
    public GameObject image;
    public Item item;
    public void ChangeColour(Color color)
    {
        button.GetComponent<SpriteRenderer>().color = color;
    }
}
