using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedualGeneration : MonoBehaviour
{

    public GameObject asteroid;
    public float radius = 10f;
    /*public float scale = 5f;
    public float modifier = 0f;*/
    public float forceScale = 100f;
    public float torqueScale = 10000f;
    // Start is called before the first frame update
    void Start()
    {
        //RandomSpawning(64, 1024, 1024);
        CreateAsteroid(0f, 0f, 10f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    
    }


    void RandomSpawning(int minimumDistance, int distanceX, int distanceY) {
        for (int x = -distanceX; x < distanceX; x += (minimumDistance * 3)) {
            for (int y = -distanceY; y < distanceY; y += (minimumDistance * 3)) {
                if (Random.value < 0.5) {
                    CreateAsteroid(Random.Range(x, x + distanceX), Random.Range(y, y + distanceY), Random.Range(10, 15), Random.Range(0, 10));
                }
            }
        }
    }

    void CreateAsteroid(float centerX, float centerY, float scale, float modifier) {
        GameObject asteroidInstance = Instantiate(asteroid, new Vector2 (centerX, centerY), Quaternion.identity);
        Rigidbody2D rb2d = asteroidInstance.GetComponent<Rigidbody2D>();

        int newNoise = Random.Range(0,10000);

        float maxPerlinValue = scale + modifier;

        AsteroidBlockControl asteroidBlockControlScript = asteroidInstance.GetComponent<AsteroidBlockControl>();

        int mass = 0;

        for (float x = centerX - maxPerlinValue; x < centerX + maxPerlinValue; x++) {
            for (float y = centerY - maxPerlinValue; y < centerY + maxPerlinValue; y++) {
                float noise = Mathf.PerlinNoise(newNoise + (x / radius), newNoise + (y / radius));

                noise *= scale;
                noise += modifier;



                if (noise - Mathf.Sqrt(Mathf.Pow(x - centerX, 2) + Mathf.Pow(y - centerY, 2)) > 0) {
                    mass += 1;

                    Vector2 blockPos = new Vector2 (x, y);
                    GameObject placedBlock = asteroidBlockControlScript.PlaceBlock(3, blockPos, false);
                    
                }
            }
        }

        //foreach(Vector2 position in asteroidBlockControlScript.asteroidBlocks.Keys) {
            //Debug.Log(position);
        //}
        //rb2d.mass = mass;
        //rb2d.AddForce(new Vector2(Random.Range(-forceScale, forceScale), Random.Range(-forceScale, forceScale)), ForceMode2D.Impulse);
        //rb2d.AddTorque(Random.Range(-torqueScale, torqueScale));
    }
}
