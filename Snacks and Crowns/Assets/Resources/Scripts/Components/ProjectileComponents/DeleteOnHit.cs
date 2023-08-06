using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnHit : MonoBehaviour
{
    public void Die(GameObject target)
    {
        if (target.GetComponent<Damagable>())
        {
            Debug.Log("Target has DAMAGABLE");
            this.gameObject.GetComponent<Projectile>().DestroyEvent();
            Destroy(this.gameObject);
        }
    }
}
