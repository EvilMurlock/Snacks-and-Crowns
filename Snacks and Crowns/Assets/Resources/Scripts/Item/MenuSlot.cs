using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuSlot : MonoBehaviour
{
    [SerializeField]
    protected Image button;
    [SerializeField]
    protected Image icon;
    protected Item item;

    private void Start()
    {
    }
    public virtual void AddItem(Item newItem)
    {
        icon.color = new Color(255, 255, 255, 255);
        item = newItem;
        icon.sprite = item.icon;
    }
    public virtual void RemoveItem()
    {
        icon.color = new Color(0, 0, 0, 0);
    }
    public bool IsEmpty()
    {
        return item == null;
    }
    public bool IsNotEmpty()
    {
        return item != null;
    }

    public Item GetItem()
    {
        return item;
    }

    public void ChangeColour(Color color)
    {
        button.GetComponent<SpriteRenderer>().color = color;
    }
    public void ChangeBackground(Sprite sprite)
    {
        icon.GetComponent<Image>().sprite = sprite;
    }

}
