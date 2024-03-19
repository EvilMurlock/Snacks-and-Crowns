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
    [SerializeField]
    GameObject goldDisplayPrefab;

    private void Awake()
    {
        //PlayerInput player_input = gameObject.GetComponent<PlayerInput>();
        PlayerInput playerInput = PlayerInput.Instantiate(playerPrefab, controlScheme: controlSheme, pairWithDevice: Keyboard.current);
        GameObject player = playerInput.gameObject;
        player.transform.SetPositionAndRotation(this.transform.position,this.transform.rotation);

        playerCamera.GetComponent<CameraFollowPlayer>().player = player;

        playerInput.uiInputModule = playerEventSystem.GetComponent<InputSystemUIInputModule>();

        player.GetComponent<MenuManager>().Innitialize(playerCanvas, playerEventSystem);

        GameObject goldDisplay = Instantiate(goldDisplayPrefab, playerCanvas.transform);
        goldDisplay.GetComponent<GoldDisplay>().CoupleToPlayer(player);
        Destroy(this.gameObject);
    }
}
