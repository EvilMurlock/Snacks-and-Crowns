using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Damagable : MonoBehaviour
{
    public UnityEvent death;
    public UnityEvent<float> healthChange;

    public float max_hp = 100;
    public float hp { get; private set; }
    Dictionary<Damage_Type, float> resistances;
    private void Start()
    {
        Changehealth(max_hp);
        resistances = new Dictionary<Damage_Type, float>();
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
        healthChange.Invoke(hp);
    }
    void Die()
    {
        Debug.Log(gameObject.name + " is DEAD");
        death.Invoke();
        Destroy(gameObject);
    }
}
public enum Damage_Type
{
    blunt, pierce, slash
}