using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regeneration : MonoBehaviour
{
    float regenerationTick = 0.3f;
    float healAmount = 1f;
    float lastRegenTime = 0;
    // Start is called before the first frame update
    Damagable damagable;
    void Start()
    {
        damagable = GetComponent<Damagable>();
    }

    // Update is called once per frame
    void Update()
    {
        if(lastRegenTime + regenerationTick < Time.timeSinceLevelLoad)
        {
            damagable.Heal(healAmount);
            lastRegenTime = Time.timeSinceLevelLoad;
        }
    }
}
