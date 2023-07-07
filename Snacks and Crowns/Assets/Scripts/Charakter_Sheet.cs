using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charakter_Sheet : MonoBehaviour
{
    [SerializeField]
    Race race;
    //Fyzical Stats
    float strenght;
    float agility;
    float size;

    //Personality Stats
    float bravery;
    float greed;

    //Derived Stats
    float hp;
    float speed;
    void Start()
    {
        //Fyzical Stats
        strenght = race.strenght;
        agility = race.agility;
        size = race.size;

        //Personality Stats
        bravery = race.bravery;
        greed = race.greed;

        //Derived Stats
        hp = size;
        speed = agility;
        gameObject.GetComponent<SpriteRenderer>().sprite = race.sprite;
    }
}
