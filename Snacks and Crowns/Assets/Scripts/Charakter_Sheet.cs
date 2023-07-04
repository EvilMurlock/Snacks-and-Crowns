using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charakter_Sheet : MonoBehaviour
{
    [SerializeField]
    Race_Data race_stats;
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
        strenght = race_stats.strenght;
        agility = race_stats.agility;
        size = race_stats.size;

        //Personality Stats
        bravery = race_stats.bravery;
        greed = race_stats.greed;

        //Derived Stats
        hp = size;
        speed = agility;
    }
}
