using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemsOnDeath : MonoBehaviour
{
    GameObject prefab;
    public List<Item> itemsToDrop;
    void Start()
    {
        prefab = (GameObject)Resources.Load("Prefabs/Items/Item");
        GetComponent<Damagable>().death.AddListener(DropItems);
    }
    void DropItems()
    {
        foreach(Item item in itemsToDrop)
        {
            GameObject item_object = Instantiate(item.prefab, transform.position, transform.rotation);
            item_object.GetComponent<Item_Controler>().item = item;
            item_object.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100),Random.Range(-100,100)).normalized * 500);
            item_object.transform.rotation = this.transform.rotation;

        }
    }

}
