using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class FactionSelect : MonoBehaviour, ISubmitHandler
{
    [SerializeField]
    TextMeshProUGUI text;

    Factions faction;

    // Start is called before the first frame update
    void Start()
    {
        faction = Factions.One;
        UpdateDisplay();
    }
    public Factions GetFaction()
    {
        return faction;
    }
    public void OnSubmit(BaseEventData _)
    {
        SelectNextFaction();
    }
    void SelectNextFaction()
    {
        if (faction == Factions.One)
            faction = Factions.Two;
        else
            faction = Factions.One;
        UpdateDisplay();
    }
    void UpdateDisplay()
    {
        text.text = faction.ToString();
    }
}
