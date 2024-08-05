using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class ReadySelect : MonoBehaviour, ISubmitHandler
{
    [SerializeField]
    GameObject shade;
    [SerializeField]
    SelectRace selectRace;
    [SerializeField]
    FaceSelect selectFace;
    [SerializeField]
    FactionSelect selectFaction;
    [HideInInspector]
    public string controlScheme;
    [HideInInspector]
    public InputDevice deviceType;
    LoadLevelWhenReady whenReady;
    [SerializeField]
    InputSystemUIInputModule uiInput;
    [SerializeField] 
    CanvasGroup canvas;
    [SerializeField]
    GameObject charakterSelectMenu;
    public void OnSubmit(BaseEventData eventData)
    {
        ReadySelf();
    }

    void ReadySelf()
    {
        shade.SetActive(true);
        //Debug.Log("Adding player with race: " + selectRace.GetSelectedRace().race + " | Face: " + selectFace.GetFace() + " | Faction: "+ selectFaction.GetFaction().ToString());
        StartGameDataHolder.AddPlayer(controlScheme, deviceType, selectRace.GetSelectedRace(), selectFaction.GetFaction(), selectFace.GetFace());
        whenReady.PlayerReady();
        charakterSelectMenu.GetComponent<PlayerInput>().DeactivateInput();
    }
    private void Start()
    {
        whenReady = FindFirstObjectByType<LoadLevelWhenReady>();
    }
}
