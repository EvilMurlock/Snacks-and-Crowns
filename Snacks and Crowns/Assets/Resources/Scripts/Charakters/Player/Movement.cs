using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


/// <summary>
/// Manages movement and rotation of a character
/// </summary>
public class Movement : MonoBehaviour
{
    [SerializeField]
    public float movementSpeed;
    public float turningSpeed;

    Rigidbody2D rigid_body;
    Vector2 movement_direction;

    public UnityEvent<bool> stun;
    private void Start()
    {       
        rigid_body = gameObject.GetComponent<Rigidbody2D>();
        movement_direction = Vector2.zero;
        ResetSpeed();
        stun.Invoke(false);
    }
    void FixedUpdate()
    {
        if (movement_direction != Vector2.zero && movementSpeed != 0) //rotates object in direction of movement
        {
            rigid_body.MovePosition((movement_direction * Time.fixedDeltaTime * movementSpeed) + (Vector2)transform.position);
            Quaternion rotate_to = Quaternion.LookRotation(Vector3.forward, movement_direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate_to, turningSpeed);
        }
    }
    public void RotateTowards(Vector3 point)
    {
        Quaternion rotate_to = Quaternion.LookRotation(Vector3.forward, point);
        Quaternion rotate_towards = Quaternion.RotateTowards(transform.rotation, rotate_to, turningSpeed);

        transform.localRotation = rotate_towards;
    }
    public void ChangeSpeed(float newSpeed, float newTurningSpeed)
    {
        movementSpeed = newSpeed;
        turningSpeed = newTurningSpeed;
    }
    public void ResetSpeed()
    {
        CharakterSheet character = GetComponent<CharakterSheet>();
        movementSpeed = character.GetSpeed();
        turningSpeed = character.GetTurningSpeed();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movement_direction = context.ReadValue<Vector2>().normalized;
    }
    public void ChangeMovementDirection(Vector2 newMovementDirection)
    {
        movement_direction = newMovementDirection;
    }
    public void MoveStop()
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
        stun.Invoke(true);
        ChangeSpeed(0, 0);
        yield return new WaitForSeconds(stunTime);
        ResetSpeed();
        stun.Invoke(false);
    }

}
