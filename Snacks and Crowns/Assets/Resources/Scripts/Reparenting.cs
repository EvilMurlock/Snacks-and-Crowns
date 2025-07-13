using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]

/// <summary>
/// Reparenting script, used to keep the scene hierarchy clean
/// </summary>
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