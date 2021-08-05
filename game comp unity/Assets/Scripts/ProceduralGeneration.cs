using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{

    public GameObject asteroid;
    public GameObject test;
    public float radius = 7f;
    /*public float scale = 5f;
    public float modifier = 0f;*/
    public float forceScale = 100f;
    public float torqueScale = 10000f;
    public Dictionary<string, float> oreRarity = new Dictionary<string, float>(){ 
        ["cobalt ore"] = 0.7f,
        ["aluminum ore"] = 0.7f,
        ["titanium ore"] = 0.3f,
        ["uranium ore"] = 0.3f,
        ["galacium ore"] = 0.1f,

    };

    // Start is called before the first frame update
    void Start()
    {  
        //RandomSpawning(64, 1024, 1024);
    }

    // Update is called once per frame
    void Update()
    {
        
    
    }


    public List<GameObject> RandomSpawning(int minimumDistance, int distanceX, int distanceY) {
        List<GameObject> asteroidList = new List<GameObject>();
        for (int x = -distanceX; x < distanceX; x += (minimumDistance * 3)) {
            for (int y = -distanceY; y < distanceY; y += (minimumDistance * 3)) {
                if ((x <= -32f || x >= 32f) && (y <= -68f || y >= 132f)) {
                    if (5625f <= (x * x + y * y) && (x * x + y * y) <= 120000f) {
                        float randomValue = Random.value;
                        if (Random.value < 0.4) {
                            asteroidList.Add(CreateAsteroid(Random.Range(x, x + minimumDistance), Random.Range(y, y + minimumDistance), Random.Range(3, 10), Random.Range(0, 7), "cobalt ore"));
                        }
                        if (0.4 <= Random.value && Random.value < 0.8) {
                            asteroidList.Add(CreateAsteroid(Random.Range(x, x + minimumDistance), Random.Range(y, y + minimumDistance), Random.Range(3, 10), Random.Range(0, 7), "aluminum ore"));
                        }
                    }
                    if (120000f < (x * x + y * y) && (x * x + y * y) <= 200000f) {
                        float randomValue = Random.value;
                        if (Random.value < 0.4) {
                            asteroidList.Add(CreateAsteroid(Random.Range(x, x + minimumDistance), Random.Range(y, y + minimumDistance), Random.Range(3, 10), Random.Range(0, 7), "titanium ore"));
                        }
                        if (0.4 <= Random.value && Random.value < 0.8) {
                            asteroidList.Add(CreateAsteroid(Random.Range(x, x + minimumDistance), Random.Range(y, y + minimumDistance), Random.Range(3, 10), Random.Range(0, 7), "uranium ore"));
                        }
                    }
                    if (200000f < (x * x + y * y) && (x * x + y * y) <= 300000f) {
                        float randomValue = Random.value;
                        if (Random.value < 0.8) {
                            asteroidList.Add(CreateAsteroid(Random.Range(x, x + minimumDistance), Random.Range(y, y + minimumDistance), Random.Range(3, 10), Random.Range(0, 7), "galacium ore"));
                        }

                    }
                }
                
            }
        }
        return asteroidList;
    }

    void SpawnOres(float minX, float maxX, float minY, float maxY, string blockID, float spread, float rarity, AsteroidBlockControl AsteroidBlockControlScript) {
        //good for now, however later we need guaranteed amount of ore spawning and making it spawn in veins
        //pick random points and then use iterative process to create a patchs
        float randomOre = Random.value;
    
        int newNoise = Random.Range(0,10000);

        for (float x = minX; x < maxX; x++) {
            for (float y = minY; y < maxY; y++) {
                float noise = Mathf.PerlinNoise(newNoise + (x / spread), newNoise + (y / spread));
                if (noise < rarity) {
                    Vector2 blockPos = new Vector2 (x, y);
                    if (AsteroidBlockControlScript.IsOccupied(blockPos)) {

                        string placedBlock = AsteroidBlockControlScript.PlaceBlock(blockID, blockPos);
                    }
                }
            }
        }
    }

    public GameObject CreateAsteroid(float centerX, float centerY, float scale, float modifier, string type) {
        GameObject asteroidInstance = Instantiate(asteroid, new Vector2 (centerX, centerY), Quaternion.identity);
        AsteroidBlockControl AsteroidBlockControlScript = asteroidInstance.GetComponent<AsteroidBlockControl>();

        Rigidbody2D rb2d = asteroidInstance.GetComponent<Rigidbody2D>();

        string asteroidType = new List<string>(){"c", "s", "m"}[Random.Range(0, 2)];

        int newNoise = Random.Range(0,10000);

        float maxPerlinValue = scale + modifier;


        int mass = 0;

        for (float x = centerX - maxPerlinValue; x < centerX + maxPerlinValue; x++) {
            for (float y = centerY - maxPerlinValue; y < centerY + maxPerlinValue; y++) {
                float noise = Mathf.PerlinNoise(newNoise + (x / radius), newNoise + (y / radius));

                noise *= scale;
                noise += modifier;

                float distanceSquared = (x - centerX) * (x - centerX) + (y - centerY) * (y - centerY);

                //change this to create a minimum and maximum distances like noise + distance > minimum

                if (noise * noise - distanceSquared > 0) {
                    Vector2 blockPos = new Vector2 (x, y);

                        mass += 1;
                        string placedBlock = AsteroidBlockControlScript.PlaceBlock("stone", blockPos);
                    
                }
            }
        }

        SpawnOres(centerX - maxPerlinValue, centerX + maxPerlinValue, centerY - maxPerlinValue, centerY + maxPerlinValue, type, 3, oreRarity[type], AsteroidBlockControlScript);

        AsteroidBlockControlScript.GenerateMesh();

        rb2d.mass = mass;
        rb2d.AddForce(new Vector2(Random.Range(-forceScale, forceScale), Random.Range(-forceScale, forceScale)), ForceMode2D.Impulse);
        rb2d.AddTorque(Random.Range(-torqueScale, torqueScale));

        return asteroidInstance;

    }
}
