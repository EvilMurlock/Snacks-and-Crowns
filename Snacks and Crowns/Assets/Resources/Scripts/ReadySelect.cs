using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


/// <summary>
/// Locks in players character selection
/// </summary>

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
    GameObject characterSelectMenu;

    int index = 0;
    public void OnSubmit(BaseEventData eventData)
    {
        ReadySelf();
    }
    public void SetIndex(int index)
    {
        this.index = index;
    }
    void ReadySelf()
    {
        shade.SetActive(true);
        StartGameDataHolder.AddPlayer(controlScheme, deviceType, selectRace.GetSelectedRace(), selectFaction.GetFaction(), selectFace.GetFace(), index);
        whenReady.PlayerReady();
        characterSelectMenu.GetComponent<PlayerInput>().DeactivateInput();
    }
    private void Start()
    {
        whenReady = FindFirstObjectByType<LoadLevelWhenReady>();
    }
}
