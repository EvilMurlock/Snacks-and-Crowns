using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
public class Player_Movement : MonoBehaviour
{
    [SerializeField]
    public float movementSpeed;
    public float turningSpeed;

    Rigidbody2D rigid_body;
    Vector2 movement_direction;

    private void Start()
    {       
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
        movement_direction = Vector2.zero;
        ResetSpeed();
    }
    void FixedUpdate()
    {
        //Debug.Log("Move speed:" + movement_speed);
        //Debug.Log("Move direction:" + movement_direction);

        if (movement_direction != Vector2.zero && movementSpeed != 0) //rotates object in direction of movement
        {
            rigid_body.MovePosition((movement_direction * Time.fixedDeltaTime * movementSpeed) + (Vector2)transform.position);
            Quaternion rotate_to = Quaternion.LookRotation(Vector3.forward, movement_direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate_to, turningSpeed);
        }
    }
    public void ChangeSpeed(float newSpeed, float newTurningSpeed)
    {
        movementSpeed = newSpeed;
        turningSpeed = newTurningSpeed;
    }
    public void ResetSpeed()
    {
        Charakter_Sheet charakter = GetComponent<Charakter_Sheet>();
        movementSpeed = charakter.GetSpeed();
        turningSpeed = charakter.GetTurningSpeed();
    }
    public void On_Move(InputAction.CallbackContext context)
    {
        movement_direction = context.ReadValue<Vector2>().normalized;
    }
    public void ChangeMovementDirection(Vector2 newMovementDirection)
    {
        movement_direction = newMovementDirection;
    }
    public void Move_Stop()
    {
        movement_direction = Vector2.zero;
    }
    public void Stun(float stunTime)
    {
        StopCoroutine("StunCo");
        StartCoroutine("StunCo", stunTime);
    }
    IEnumerator StunCo(float stunTime)
    {
        ChangeSpeed(0, 0);
        yield return new WaitForSeconds(stunTime);
        ResetSpeed();
    }

}
