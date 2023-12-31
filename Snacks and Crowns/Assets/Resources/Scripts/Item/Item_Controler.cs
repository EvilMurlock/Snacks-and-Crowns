using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class Item_Controler : MonoBehaviour
{
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.icon;
        item.prefab = (GameObject)Resources.Load("Prefabs/Items/Item");
        if (!gameObject.GetComponent<TagSystem>())
        {
            gameObject.AddComponent<TagSystem>();
        }
        gameObject.GetComponent<TagSystem>().AddTags(item.Tags);
    }
}
