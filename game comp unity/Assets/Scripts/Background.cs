using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject background;

    void Start()
    {
        for (int x = -1024; x<1024; x+=64) {
            for (int y = -1024; y<1024; y+=64) {
                Instantiate(background, new Vector2(x, y), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
