using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    Damagable damagable;
    void Start()
    {
        damagable = GetComponent<Damagable>();
        damagable.death.AddListener(Respawn);
    }
    void Respawn()
    {
        //damagable.Set damagable.max_hp)
    }
}
