using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;



// manages the face selection in the character selection menu
public class FaceSelect : MonoBehaviour, ISubmitHandler
{
    Race race;
    int faceIndex = 0;
    [SerializeField] 
    Image face;

    // Start is called before the first frame update
    void Start()
    {
        race = (Race)Resources.LoadAll("Races")[0];
        UpdateDisplay();
    }
    public int GetFace()
    {
        return faceIndex;
    }
    public void ChangeRace(Race newRace)
    {
        race = newRace;
        faceIndex = 0;
        UpdateDisplay();
    }
    public void OnSubmit(BaseEventData _)
    {
        SelectNextFace();
    }
    void SelectNextFace()
    {
        faceIndex = (faceIndex + 1) % race.faces.Count;
        UpdateDisplay();
    }
    void UpdateDisplay()
    {
        face.sprite = race.faces[faceIndex];
    }
}
