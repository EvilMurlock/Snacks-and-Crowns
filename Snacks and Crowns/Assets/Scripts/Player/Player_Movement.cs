using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField]
    float movement_speed = 10f;

    Rigidbody2D rigid_body;
    Vector2 movement_direction;

    private void Start()
    {
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
        movement_direction = Vector2.zero;
    }
    void FixedUpdate()
    {
        rigid_body.MovePosition((movement_direction * Time.deltaTime * movement_speed) + (Vector2) transform.position);

        if (movement_direction != Vector2.zero) //rotates object in direction of movement
        {
            Quaternion rotate_to = Quaternion.LookRotation(Vector3.forward, movement_direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate_to, 20);
        }
    }
    public void On_Move(InputAction.CallbackContext context)
    {
        movement_direction = context.ReadValue<Vector2>().normalized;
    }
    public void Move_Stop()
    {
        movement_direction = Vector2.zero;
    }
}
