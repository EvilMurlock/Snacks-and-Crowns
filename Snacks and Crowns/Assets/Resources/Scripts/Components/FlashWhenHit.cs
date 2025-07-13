using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Flashes sprite with a colour when hit
/// </summary>
public class FlashWhenHit : MonoBehaviour
{
    // Start is called before the first frame update
    float previousHp;
    int flashes = 1;
    float flashDuration = 0.1f;
    [SerializeField]
    Color flashColour = Color.red;
    void Start()
    {
        var damagable = GetComponent<Damageable>();
        damagable.healthChange.AddListener(FlashRed);
        previousHp = damagable.hp;
    }

    void FlashRed(float curentHp, float maxHp)
    {
        if (curentHp < previousHp)
        {
            StartCoroutine("Flash", flashColour);
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
