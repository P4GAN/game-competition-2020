using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (WorldBuilder.player != null) {
            gameObject.transform.position = new Vector3(WorldBuilder.player.transform.position.x, WorldBuilder.player.transform.position.y, WorldBuilder.player.transform.position.z - 10);
        }
    }
}
