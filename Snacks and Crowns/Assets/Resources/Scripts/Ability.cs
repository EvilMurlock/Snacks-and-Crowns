using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Ability", menuName = "Ability")]

public class Ability : ScriptableObject
{
    public float cooldown;
    [SerializeReference]
    List<ComponentDataGeneric> abilityComponents;
    public virtual void Use(GameObject user)
    {
        foreach (ComponentDataGeneric comData in abilityComponents)
        {
            comData.InicializeComponent(user);
        }
    }
    public void AddAbilityComponent(ComponentDataGeneric data)
    {
        abilityComponents.Add(data);
    }
}
