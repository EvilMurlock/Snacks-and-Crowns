using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharakterSheet : MonoBehaviour
{
    [SerializeField]
    Race race;

    
    //Fyzical Stats

    //Derived Stats
    void Awake()
    {
        //Derived Stats
        SetFace();
        
    }
    public void SetRace(Race race, int face)
    {
        this.race = race;
        SetFace(face);
    }
    void SetFace()
    {
        SetFace(Random.Range(0, race.faces.Count - 1));
    }

    void SetFace(int faceIndex)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = race.faces[faceIndex];
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
