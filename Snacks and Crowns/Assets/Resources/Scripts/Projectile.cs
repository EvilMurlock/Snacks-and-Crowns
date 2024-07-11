using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeReference]
    public List<ComponentDataGeneric> componentData;
    [HideInInspector]
    public UnityEvent<GameObject> onHit;
    [HideInInspector]
    public UnityEvent<GameObject> onDestroy;
    List<GameObject> colidedObjects = new List<GameObject>();
    [HideInInspector]
    public Vector2 direction;
    [HideInInspector]
    public float speed;
    void Start()
    {
        foreach (ComponentDataGeneric compData in componentData)
        {
            compData.InicializeComponent(gameObject);
        }
    }
    private void Update()
    {
        this.transform.position = this.transform.position + (Vector3)direction * speed * Time.deltaTime;
    }
    public void AddData(ComponentDataGeneric data)
    {
        componentData.Add(data);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("HITING: " + collision.gameObject.name);
        GameObject colObject = collision.gameObject;
        //Isnt the weilder, can be hit and has not yet been hit
        if (!colidedObjects.Contains(colObject) &&
            !collision.isTrigger)
        {
            onHit.Invoke(collision.gameObject);
        }
    }
    public void DestroyEvent()
    {
        onDestroy.Invoke(this.gameObject);
    }
    private void OnDestroy()
    {
    }
}
