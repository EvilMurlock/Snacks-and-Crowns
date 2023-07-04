using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Reparenting : MonoBehaviour
{
    public string parentObjectName;
    private bool isPlaced = false;
    private void Awake()
    {
        if (Application.isEditor && !isPlaced)
        {
            transform.parent = GameObject.Find(parentObjectName).transform;
            isPlaced = true;
        }
    }
}