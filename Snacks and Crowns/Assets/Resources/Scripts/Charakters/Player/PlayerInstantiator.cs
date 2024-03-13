using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class Player_Instantiator : MonoBehaviour
{

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    string controlSheme;

    [SerializeField]
    GameObject playerCamera;
    [SerializeField]
    MultiplayerEventSystem playerEventSystem;
    [SerializeField]
    Canvas playerCanvas;


    private void Awake()
    {
        //PlayerInput player_input = gameObject.GetComponent<PlayerInput>();
        PlayerInput playerInput = PlayerInput.Instantiate(playerPrefab, controlScheme: controlSheme, pairWithDevice: Keyboard.current);
        playerInput.gameObject.transform.SetPositionAndRotation(this.transform.position,this.transform.rotation);

        playerCamera.GetComponent<CameraFollowPlayer>().player = playerInput.transform.gameObject;

        playerInput.uiInputModule = playerEventSystem.GetComponent<InputSystemUIInputModule>();

        playerInput.gameObject.GetComponent<MenuManager>().Innitialize(playerCanvas, playerEventSystem);
        Destroy(this.gameObject);
    }
}
