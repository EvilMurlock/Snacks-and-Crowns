using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HealthTracker : MonoBehaviour
{
    public GameObject healthBar;
    Slider slider;
    public AnimationClip barAnimation;
    // Start is called before the first frame update
    void Awake()
    {
        slider = healthBar.GetComponent<Slider>();
        healthBar.GetComponentInChildren<Animator>().Play(barAnimation.name);
    }
    public void CoupleToPlayer(GameObject player)
    {
        player.GetComponent<Damagable>().healthChange.AddListener(UpdateHealthCounter);
        UpdateHealthCounter(player.GetComponent<Damagable>().hp, player.GetComponent<Damagable>().max_hp);
    }
    void UpdateHealthCounter(float health, float maxHealth)
    {
       slider.value = health/maxHealth;
        //text.text = health.ToString();
    }
}
