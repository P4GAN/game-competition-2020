using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // Start is called before the first frame update

    public float backgroundWidth;
    public float backgroundHeight;
    public Vector2 startPosition;
    public List<GameObject> backgroundPrefabs;
    public List<float> speeds;
    public List<GameObject> layerList = new List<GameObject>();
    public Rigidbody2D rb2d;

    void Start()
    {
        rb2d = WorldBuilder.player.GetComponent<Rigidbody2D>();
        startPosition = WorldBuilder.player.transform.position;
        for (int i = 0; i < speeds.Count; i++) {
            layerList.Add(Instantiate(backgroundPrefabs[i], startPosition, Quaternion.identity)); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < layerList.Count; i++) {
            layerList[i].transform.Translate(-rb2d.velocity * speeds[i]);

            if (Mathf.Abs(transform.position.x - layerList[i].transform.position.x) >= backgroundWidth) {
                float offsetPositionX = (transform.position.x - layerList[i].transform.position.x) % backgroundWidth;
                layerList[i].transform.position = new Vector3(transform.position.x + offsetPositionX, layerList[i].transform.position.y);
            }

            if (Mathf.Abs(transform.position.y - layerList[i].transform.position.y) >= backgroundHeight) {
                float offsetPositionY = (transform.position.y - layerList[i].transform.position.y) % backgroundHeight;
                layerList[i].transform.position = new Vector3(layerList[i].transform.position.x, transform.position.y + offsetPositionY);
            }
    
        }
    }
}
