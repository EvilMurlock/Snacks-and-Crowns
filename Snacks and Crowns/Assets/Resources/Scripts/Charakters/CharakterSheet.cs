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
        RaceInnit();
    }
    public void SetRace(Race race, int face)
    {
        this.race = race;
        SetFace(face);
        RaceInnit();
    }
    void RaceInnit()
    {
        Damageable damagable = GetComponent<Damageable>();
        foreach (ResistanceData data in race.resistances)
        {
            damagable.ChangeResistance(data.resistanceType, data.resistanceValue);
        }
    }
    void SetFace()
    {
        SetFace(Random.Range(0, race.faces.Count));
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
