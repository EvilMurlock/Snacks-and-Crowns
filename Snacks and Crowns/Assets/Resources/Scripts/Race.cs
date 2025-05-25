using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New_Race", menuName = "Race")]
public class Race : ScriptableObject
{
    //Esthetics
    public List<Sprite> faces;
    public string race;

    //Fyzical Stats
    public float strenght;
    public float agility;
    public float size;
    public float magic;
    //Personality Stats
    public float bravery;
    public float greed;

    public List<ResistanceData> resistances;
}
