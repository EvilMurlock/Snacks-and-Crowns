using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Initializes character stats from the race
/// </summary>
public class CharacterSheet : MonoBehaviour
{
    [SerializeField]
    Race race;

   
    void Awake()
    {
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
        return race.size * 10 + race.strenght*10;
    }
}
