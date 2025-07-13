using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


// class that manages health and broadcasts health changes and death
public class Damageable : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent death;
    [HideInInspector]
    public UnityEvent<float,float> healthChange;

    public float max_hp = 100;
    public float hp { get; private set; }
    Dictionary<DamageType, float> resistances = new Dictionary<DamageType, float>();
    private void Start()
    {
        ChangeHealth(max_hp);
    }
    public void TakeDamage(Attack attack)
    {
        float resistance = 0;
        if (resistances.ContainsKey(attack.damage_type))
            { resistance = resistances[attack.damage_type]; }
        float damageTaken = attack.damage * Mathf.Clamp(1 - resistance, 0, 1);
        ChangeHealth(hp-damageTaken);
        if (hp <= 0) 
            Die();
    }
    public void Heal(float healAmount)
    {
        ChangeHealth(hp + healAmount);
    }
    void ChangeHealth(float newHealth)
    {
        hp = Mathf.Min(newHealth, max_hp);
        healthChange.Invoke(hp,max_hp);
    }
    void Die()
    {
        death.Invoke();
        if(hp<=0)
            Destroy(gameObject);
    }
    public void ChangeResistance(DamageType type, float amount)
    {
        if (!resistances.TryAdd(type, amount))
            resistances[type] += amount;
    }
}
public enum DamageType
{
    blunt, pierce, slash, fire, ice, magic
}