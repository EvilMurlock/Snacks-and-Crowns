using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deletes this on hit
/// </summary>
public class DeleteOnHit : MonoBehaviour
{
    public void Die(GameObject target)
    {
        if (target.GetComponent<Damageable>())
        {
            this.gameObject.GetComponent<Projectile>().DestroyEvent();
            Destroy(this.gameObject);
        }
    }
}
