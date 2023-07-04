using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class Player_Instantiator : MonoBehaviour
{

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    string control_sheme;

    [SerializeField]
    GameObject player_camera;
    [SerializeField]
    GameObject player_event_system;
    [SerializeField]
    Canvas player_canvas;


    private void Start()
    {
        //PlayerInput player_input = gameObject.GetComponent<PlayerInput>();
        PlayerInput player_input = PlayerInput.Instantiate(playerPrefab, controlScheme: control_sheme, pairWithDevice: Keyboard.current);
        player_input.gameObject.transform.SetPositionAndRotation(this.transform.position,this.transform.rotation);

        player_camera.GetComponent<Camera_Follow_Player>().player = player_input.transform.gameObject;

        player_input.uiInputModule = player_event_system.GetComponent<InputSystemUIInputModule>();
        player_input.gameObject.GetComponent<Player_Inventory>().canvas = player_canvas;
        player_input.gameObject.GetComponent<Player_Inventory>().event_system = player_event_system;

        Destroy(this.gameObject);
    }
}
