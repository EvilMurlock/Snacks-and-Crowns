using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 new_position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            transform.position = new_position;
        }
    }
}
