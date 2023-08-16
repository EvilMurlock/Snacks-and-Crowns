using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterTime : MonoBehaviour
{
    public float time;
    public void Init(float time)
    {
        this.time = time;

    }
    private void Start()
    {
        StartCoroutine("destroySelf");
    }
    IEnumerator destroySelf()
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
