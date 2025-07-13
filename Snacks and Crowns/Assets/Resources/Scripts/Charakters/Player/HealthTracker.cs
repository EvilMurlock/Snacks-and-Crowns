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
    [SerializeField]
    GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        slider = healthBar.GetComponent<Slider>();
        CoupleToPlayer(player);
    }
    public void CoupleToPlayer(GameObject newPlayer)
    {
        this.player = newPlayer;
        healthBar.GetComponentInChildren<Animator>().Play(barAnimation.name);
        player.GetComponent<Damageable>().healthChange.AddListener(UpdateHealthCounter);
        UpdateHealthCounter(player.GetComponent<Damageable>().hp, player.GetComponent<Damageable>().max_hp);
    }
    void UpdateHealthCounter(float health, float maxHealth)
    {
       slider.value = health/maxHealth;
        //text.text = health.ToString();
    }
}
