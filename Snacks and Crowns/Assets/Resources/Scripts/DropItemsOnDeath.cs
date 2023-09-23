using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
public class DropItemsOnDeath : MonoBehaviour
{
    public List<Item> itemsToDrop;
    void Start()
    {
        GetComponent<Damagable>().death.AddListener(DropItems);
        if (!gameObject.GetComponent<TagSystem>())
        {
            gameObject.AddComponent<TagSystem>();
        }
        foreach(Item i in itemsToDrop)
        {
            gameObject.GetComponent<TagSystem>().AddTag(i.name+"Drop");
        }
    }
    void DropItems()
    {
        foreach(Item item in itemsToDrop)
        {
            GameObject item_object = Instantiate((GameObject)Resources.Load("Prefabs/Items/Item"), transform.position, transform.rotation);
            item_object.GetComponent<Item_Controler>().item = item;
            item_object.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100),Random.Range(-100,100)).normalized * 500);
            item_object.transform.rotation = this.transform.rotation;

        }
    }

}
