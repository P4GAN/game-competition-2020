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
                if (Random.value < 0.5) {
                    asteroidList.Add(CreateAsteroid(Random.Range(x, x + distanceX), Random.Range(y, y + distanceY), Random.Range(10, 15), Random.Range(0, 10)));
                    //asteroidList.Add(Instantiate(test, new Vector2(x, y), Quaternion.identity));
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
                if (noise > rarity) {
                    Vector2 blockPos = new Vector2 (x, y);
                    if (AsteroidBlockControlScript.IsOccupied(blockPos)) {

                        GameObject placedBlock = AsteroidBlockControlScript.PlaceBlock(blockID, blockPos);
                    }
                }
            }
        }
    }

    public GameObject CreateAsteroid(float centerX, float centerY, float scale, float modifier) {
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
                        GameObject placedBlock = AsteroidBlockControlScript.PlaceBlock("stone", blockPos);
                    
                }
            }
        }

        SpawnOres(centerX - maxPerlinValue, centerX + maxPerlinValue, centerY - maxPerlinValue, centerY + maxPerlinValue, "iron ore", 3, 0.7f, AsteroidBlockControlScript);
        SpawnOres(centerX - maxPerlinValue, centerX + maxPerlinValue, centerY - maxPerlinValue, centerY + maxPerlinValue, "copper ore", 3, 0.7f, AsteroidBlockControlScript);
        SpawnOres(centerX - maxPerlinValue, centerX + maxPerlinValue, centerY - maxPerlinValue, centerY + maxPerlinValue, "aluminum ore", 3, 0.7f, AsteroidBlockControlScript);
        SpawnOres(centerX - maxPerlinValue, centerX + maxPerlinValue, centerY - maxPerlinValue, centerY + maxPerlinValue, "cobalt ore", 3, 0.7f, AsteroidBlockControlScript);
        SpawnOres(centerX - maxPerlinValue, centerX + maxPerlinValue, centerY - maxPerlinValue, centerY + maxPerlinValue, "coal ore", 3, 0.7f, AsteroidBlockControlScript);
        SpawnOres(centerX - maxPerlinValue, centerX + maxPerlinValue, centerY - maxPerlinValue, centerY + maxPerlinValue, "magnesium ore", 3, 0.7f, AsteroidBlockControlScript);

        return asteroidInstance;

        //foreach(Vector2 position in AsteroidBlockControlScript.asteroidBlocks.Keys) {
            //Debug.Log(position);
        //}
        //rb2d.mass = mass;
        //rb2d.AddForce(new Vector2(Random.Range(-forceScale, forceScale), Random.Range(-forceScale, forceScale)), ForceMode2D.Impulse);
        //rb2d.AddTorque(Random.Range(-torqueScale, torqueScale));
    }
}
