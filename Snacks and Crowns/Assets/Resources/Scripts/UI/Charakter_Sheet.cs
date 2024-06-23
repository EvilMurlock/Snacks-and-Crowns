using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charakter_Sheet : MonoBehaviour
{
    [SerializeField]
    Race race;

    
    //Fyzical Stats

    int faction;
    //Derived Stats
    void Start()
    {
        //Derived Stats
        gameObject.GetComponent<SpriteRenderer>().sprite = race.sprite;
    }
    public float GetSpeed()
    {
        return race.agility/2;
    }
    public float GetTurningSpeed()
    {
        return race.agility * 2;
    }
    public float GetMaxHP()
    {
        return race.size * 10;
    }
}
