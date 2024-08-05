using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InputField))]
public class SelectRace : MonoBehaviour, ISubmitHandler
{
    [SerializeField]
    Image face;
    [SerializeField]
    TextMeshProUGUI raceText;
    List<Race> races = new List<Race>();
    [SerializeField]
    FaceSelect faceSelect;
    int raceIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach (object o in Resources.LoadAll("Races"))
        {
            races.Add((Race)o);
        }
        UpdateDisplay();
    }
    public void OnSubmit(BaseEventData _)
    {
        SelectNextRace();
    }
    void SelectNextRace()
    {
        raceIndex = (raceIndex + 1) % races.Count;
        UpdateDisplay();
    }
    void UpdateDisplay()
    {
        raceText.text = races[raceIndex].race;
        faceSelect.ChangeRace(races[raceIndex]);
    }
    public Race GetSelectedRace()
    {
        return races[raceIndex];
    }
}
