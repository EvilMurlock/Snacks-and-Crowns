using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField]
    public float movement_speed;
    public float movementSpeed;
    public float turningSpeed = 20;

    Rigidbody2D rigid_body;
    Vector2 movement_direction;

    private void Start()
    {
        movementSpeed = movement_speed;
       
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
        movement_direction = Vector2.zero;
    }
    void FixedUpdate()
    {
        //Debug.Log("Move speed:" + movement_speed);
        //Debug.Log("Move direction:" + movement_direction);

        if (movement_direction != Vector2.zero && movement_speed!=0) //rotates object in direction of movement
        {
            rigid_body.MovePosition((movement_direction * Time.fixedDeltaTime * movement_speed) + (Vector2)transform.position);
            Quaternion rotate_to = Quaternion.LookRotation(Vector3.forward, movement_direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate_to, turningSpeed);
        }
    }
    public void ChangeSpeed(float newSpeed, float newTurningSpeed)
    {
        movement_speed = newSpeed;
        turningSpeed = newTurningSpeed;
    }
    public void ResetSpeed()
    {
        movement_speed = movementSpeed;
        turningSpeed = 20;

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
