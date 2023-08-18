using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class Damagable : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent death;
    [HideInInspector]
    public UnityEvent<float,float> healthChange;

    public float max_hp = 100;
    public float hp { get; private set; }
    Dictionary<Damage_Type, float> resistances;
    private void Start()
    {
        Changehealth(max_hp);
        resistances = new Dictionary<Damage_Type, float>();
        foreach(Damage_Type type in Enum.GetValues(typeof(Damage_Type)))
        {
            resistances[type] = 0;
        }
    }
    public void TakeDamage(Attack attack)
    {
        float resistance = 0;
        if (resistances.ContainsKey(attack.damage_type))
        { resistance = resistances[attack.damage_type]; }
        float damageTaken = attack.damage * Mathf.Clamp(1 - resistance, 0, 1);
        Debug.Log("Damage taken: " + damageTaken);
        Changehealth(hp-damageTaken);
        if (hp < 0) Die();
    }
    public void Heal(float healAmount)
    {
        Changehealth(hp + healAmount);
    }
    void Changehealth(float newHealth)
    {
        hp = newHealth;
        healthChange.Invoke(hp,max_hp);
    }
    void Die()
    {
        Debug.Log(gameObject.name + " is DEAD");
        death.Invoke();
        Destroy(gameObject);
    }
    public void ChangeResistance(Damage_Type type, float amount)
    {
        resistances[type] += amount;
        Debug.Log("Resistance changed: " + type.ToString());
        Debug.Log("New resistance value: "+ resistances[type]);
    }
}
public enum Damage_Type
{
    blunt, pierce, slash
}