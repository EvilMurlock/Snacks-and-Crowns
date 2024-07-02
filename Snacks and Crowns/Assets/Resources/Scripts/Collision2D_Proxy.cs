using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collision2D_Proxy : MonoBehaviour
{

    public Action<Collider2D> OnTriggerEnter2D_Action;
    public Action<Collider2D> OnTriggerExit2D_Action;


    public Action<Collision2D> OnCollisionEnter2D_Action;
    public Action<Collision2D> OnCollisionStay2D_Action;
    public Action<Collision2D> OnCollisionExit2D_Action;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter2D_Action?.Invoke(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit2D_Action?.Invoke(collision);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnter2D_Action?.Invoke(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionStay2D_Action?.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionExit2D_Action?.Invoke(collision);
    }
}