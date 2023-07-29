using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HealthTracker : MonoBehaviour
{
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Awake()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }
    public void CoupleToPlayer(GameObject player)
    {
        player.GetComponent<Damagable>().healthChange.AddListener(UpdateHealthCounter);
        UpdateHealthCounter(player.GetComponent<Damagable>().hp);
    }
    void UpdateHealthCounter(float health)
    {
        text.text = health.ToString();
    }
}
