using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Heals character by x each frame
/// </summary>
public class Regeneration : MonoBehaviour
{
    float regenerationTick = 0.3f;
    float healAmount = 1f;
    float lastRegenTime = 0;
    Damageable damagable;
    void Start()
    {
        damagable = GetComponent<Damageable>();
    }

    void Update()
    {
        if(lastRegenTime + regenerationTick < Time.timeSinceLevelLoad)
        {
            damagable.Heal(healAmount);
            lastRegenTime = Time.timeSinceLevelLoad;
        }
    }
}
