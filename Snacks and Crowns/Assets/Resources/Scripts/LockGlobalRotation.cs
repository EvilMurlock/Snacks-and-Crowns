using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockGlobalRotation : MonoBehaviour
{

    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 1, 0));
    }
}
