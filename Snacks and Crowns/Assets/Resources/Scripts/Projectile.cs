using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public List<ComponentDataGeneric> componentData;
    UnityEvent<GameObject> onHit;
    void Start()
    {
        foreach (ComponentDataGeneric compData in componentData)
        {
            compData.InicializeComponent(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onHit.Invoke(collision.gameObject);
    }
}
