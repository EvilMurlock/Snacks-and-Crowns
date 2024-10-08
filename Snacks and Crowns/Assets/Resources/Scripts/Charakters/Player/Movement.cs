using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
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
        //Debug.Log("Move speed:" + movement_speed);
        //Debug.Log("Move direction:" + movement_direction);

        if (movement_direction != Vector2.zero && movementSpeed != 0) //rotates object in direction of movement
        {
            rigid_body.MovePosition((movement_direction * Time.fixedDeltaTime * movementSpeed) + (Vector2)transform.position);
            Quaternion rotate_to = Quaternion.LookRotation(Vector3.forward, movement_direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate_to, turningSpeed);
        }
    }
    public void RotateTowars(Vector3 point)
    {
        //Debug.Log("Rotating to: " + point);
        Quaternion rotate_to = Quaternion.LookRotation(Vector3.forward, point);
        //Debug.Log("Rotate_to: " + rotate_to);
        Quaternion rotate_towards = Quaternion.RotateTowards(transform.rotation, rotate_to, turningSpeed);
        //Debug.Log("Rotate_towards: " + rotate_towards);

        transform.localRotation = rotate_towards;
    }
    public void ChangeSpeed(float newSpeed, float newTurningSpeed)
    {
        movementSpeed = newSpeed;
        turningSpeed = newTurningSpeed;
    }
    public void ResetSpeed()
    {
        CharakterSheet charakter = GetComponent<CharakterSheet>();
        movementSpeed = charakter.GetSpeed();
        turningSpeed = charakter.GetTurningSpeed();
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
