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
    public void AddItem(Item newItem)
    {
        if (newItem == null) item = null;
        else item = newItem;
        UpdateItem();
    }
    public void RemoveItem()
    {
        item = null;
        UpdateItem();
    }
    public virtual void UpdateItem()
    {
        if (item == null) icon.color = new Color(0, 0, 0, 0);
        else
        {
            icon.color = new Color(1, 1, 1, 1);
            icon.sprite = item.icon;
        }
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
        //color.a = 1;
        button.color = color;
    }
    public void ChangeBackground(Sprite sprite)
    {
        icon.GetComponent<Image>().sprite = sprite;
    }

}
