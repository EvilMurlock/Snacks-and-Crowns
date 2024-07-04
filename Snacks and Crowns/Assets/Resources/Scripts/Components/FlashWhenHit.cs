using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlashWhenHit : MonoBehaviour
{
    // Start is called before the first frame update
    float previousHp;
    int flashes = 1;
    float flashDuration = 0.1f;
    void Start()
    {
        var damagable = GetComponent<Damagable>();
        damagable.healthChange.AddListener(FlashRed);
        previousHp = damagable.hp;
    }

    void FlashRed(float curentHp, float maxHp)
    {
        if (curentHp < previousHp)
        {
            StartCoroutine("Flash", Color.red);
        }
        previousHp = curentHp;
    }

    IEnumerator Flash(Color color)
    {
        var renderer = GetComponent<SpriteRenderer>();
        for (int i = 0; i < flashes; i++)
        {
            renderer.color = color;
            yield return new WaitForSeconds(flashDuration);
            renderer.color = Color.white;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}
