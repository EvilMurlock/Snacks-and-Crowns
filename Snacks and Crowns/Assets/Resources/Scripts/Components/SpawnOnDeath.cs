using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDeath : MonoBehaviour
{
    public GameObject GameObjectToSpawn;
    void Start()
    {
        GetComponent<Damageable>().death.AddListener(DropItems);
    }
    void DropItems()
    {
        GameObject item_object = Instantiate(GameObjectToSpawn, transform.position, transform.rotation);
    }


}
