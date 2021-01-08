using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public float gravityForce;

    void Start()
    {
        CreatePlanet(28);
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D rb2d = player.GetComponent<Rigidbody2D>();
        Vector2 direction = (transform.position - player.transform.position).normalized;
        rb2d.AddForce(direction * gravityForce);
        float velocity = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;
        player.transform.up = -direction;
    }

    void CreatePlanet(int radius) {
        AsteroidBlockControl AsteroidBlockControlScript = gameObject.GetComponent<AsteroidBlockControl>();

        for (float x = -radius; x < radius; x++) {
            for (float y = -radius; y < radius; y++) {

                float distance = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));

                if (distance <= radius) {
                    GameObject placedBlock = AsteroidBlockControlScript.PlaceBlock(3, new Vector2 (x, y), false);
                    
                }
            }
        }
    }
}
