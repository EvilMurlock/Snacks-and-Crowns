using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuSlot : MonoBehaviour
{
    protected GameObject button;
    protected GameObject image;
    protected Item item;
    public void ChangeColour(Color color)
    {
        button.GetComponent<SpriteRenderer>().color = color;
    }
    public void ChangeBackground(Sprite sprite)
    {
        image.GetComponent<Image>().sprite = sprite;
    }

}
