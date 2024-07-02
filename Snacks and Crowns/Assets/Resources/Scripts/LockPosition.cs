using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPosition : MonoBehaviour
{
    [SerializeField]
    Vector3 offset;
    // Update is called once per frame
    void Update()
    {
        transform.position = transform.parent.position + offset;
    }
}
