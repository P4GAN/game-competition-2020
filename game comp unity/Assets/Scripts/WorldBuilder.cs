using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[Serializable] 
public class BlockObject {
    public float gridX;
    public float gridY;
    public string blockID;
}

[Serializable]
public class AsteroidObject {
    public float x;
    public float y;
    public float angle;
    public float velocityX;
    public float velocityY;
    public float angularVelocity;
    public List<BlockObject> blockList;

}
[Serializable]
public class PlayerObject {
    public float x;
    public float y;
    public float angle;
    public float velocityX;
    public float velocityY;
    public float angularVelocity;
    public List<Item> InventoryItems;
}

[Serializable]
public class WorldObject {
    public int seed;
    public PlayerObject playerObject;
    public List<AsteroidObject> asteroidList;
}

public class WorldBuilder : MonoBehaviour
{
    public int seed;    
    public GameObject setAsteroidGameObject;
    public static GameObject asteroidGameObject;
    public static List<GameObject> asteroidGameObjectList = new List<GameObject>();
    public static GameObject player;
    public GameObject playerPrefab;
    public ProceduralGeneration proceduralGenerationScript;

    void Awake() {
        Debug.Log("W");
        asteroidGameObject = setAsteroidGameObject;
        if (File.Exists("world.json")) {
            LoadWorld("world.json");

        }
        else {
            GenerateWorld();
        }
        UnityEngine.Random.InitState(seed);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            SaveWorld(asteroidGameObjectList, player, seed);
        }
    }

    void SaveWorld(List<GameObject> AsteroidList, GameObject Player, int seed) {
        WorldObject worldObject = new WorldObject();
        worldObject.seed = seed;
        worldObject.asteroidList = new List<AsteroidObject>();

        foreach (GameObject asteroid in AsteroidList) {

            AsteroidObject asteroidObject = new AsteroidObject();
            asteroidObject.x = asteroid.transform.position.x;
            asteroidObject.y = asteroid.transform.position.y;
            asteroidObject.angle = asteroid.transform.eulerAngles.z;
            asteroidObject.velocityX = asteroid.GetComponent<Rigidbody2D>().velocity.x;
            asteroidObject.velocityY = asteroid.GetComponent<Rigidbody2D>().velocity.y;
            asteroidObject.angularVelocity = asteroid.GetComponent<Rigidbody2D>().angularVelocity;            
            asteroidObject.blockList = new List<BlockObject>();

            foreach (Vector2 position in asteroid.GetComponent<AsteroidBlockControl>().asteroidBlocks.Keys) {

                BlockObject blockObject = new BlockObject();
                blockObject.gridX = position.x;
                blockObject.gridY = position.y;
                blockObject.blockID = asteroid.GetComponent<AsteroidBlockControl>().asteroidBlocks[position].GetComponent<ItemData>().item.itemID;
                asteroidObject.blockList.Add(blockObject);

            }

            worldObject.asteroidList.Add(asteroidObject);
        }

        PlayerObject playerObject = new PlayerObject();
        playerObject.x = Player.transform.position.x;
        playerObject.y = Player.transform.position.y;
        playerObject.angle = Player.transform.eulerAngles.z;
        playerObject.velocityX = Player.GetComponent<Rigidbody2D>().velocity.x;
        playerObject.velocityY = Player.GetComponent<Rigidbody2D>().velocity.y;
        playerObject.angularVelocity = Player.GetComponent<Rigidbody2D>().angularVelocity;
        playerObject.InventoryItems = Player.GetComponent<Inventory>().InventoryItems;

        worldObject.playerObject = playerObject;

        string json = JsonUtility.ToJson(worldObject, true);
        File.WriteAllText("world.json", json);
    }


    void LoadWorld(string fileName) {
        string json = File.ReadAllText(fileName);
        WorldObject worldObject = JsonUtility.FromJson<WorldObject>(json);
        seed = worldObject.seed;

        foreach (AsteroidObject asteroidObject in worldObject.asteroidList) {
            GameObject asteroidGameObjectInstance = Instantiate(asteroidGameObject, new Vector2(asteroidObject.x, asteroidObject.y), Quaternion.Euler(0, 0, asteroidObject.angle));
            AsteroidBlockControl asteroidBlockControlScript = asteroidGameObjectInstance.GetComponent<AsteroidBlockControl>();
            foreach (BlockObject blockObject in asteroidObject.blockList) {
                GameObject blockObjectInstance = asteroidBlockControlScript.PlaceBlock(blockObject.blockID, asteroidBlockControlScript.gridPositionToGamePosition(new Vector2(blockObject.gridX, blockObject.gridY))); 
            }

            asteroidBlockControlScript.rb2d.velocity = new Vector2(asteroidObject.velocityX, asteroidObject.velocityY);
            asteroidBlockControlScript.rb2d.angularVelocity = asteroidObject.angularVelocity;
            asteroidGameObjectList.Add(asteroidGameObjectInstance);
        }

        GameObject playerGameObjectInstance = Instantiate(playerPrefab, new Vector2(worldObject.playerObject.x, worldObject.playerObject.y), Quaternion.Euler(0, 0, worldObject.playerObject.angle));
        playerGameObjectInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(worldObject.playerObject.velocityX, worldObject.playerObject.velocityY);
        playerGameObjectInstance.GetComponent<Rigidbody2D>().angularVelocity = worldObject.playerObject.angularVelocity;
        playerGameObjectInstance.GetComponent<Inventory>().InventoryItems = worldObject.playerObject.InventoryItems;
        playerGameObjectInstance.name = "player";
        player = playerGameObjectInstance;

        player.GetComponent<Inventory>().UpdateInventoryUI();

        ItemControl.selectedAsteroid = asteroidGameObjectList[0];
        ItemControl.AsteroidBlockControlScript = ItemControl.selectedAsteroid.GetComponent<AsteroidBlockControl>();
    }

    void GenerateWorld() {
        seed = UnityEngine.Random.Range(-2147483647, 2147483647);

        player = Instantiate(playerPrefab, new Vector2(0, 15), Quaternion.identity);
        player.name = "player";
        player.GetComponent<Inventory>().AddItem("pickaxe", 1);
        player.GetComponent<Inventory>().UpdateInventoryUI();

        proceduralGenerationScript = GetComponent<ProceduralGeneration>();
            
        float startTime = Time.realtimeSinceStartup;

        //asteroidGameObjectList = proceduralGenerationScript.RandomSpawning(64, 512, 512);
        asteroidGameObjectList.Add(proceduralGenerationScript.CreateAsteroid(0f, 0f, 10f, 5f));



        SaveWorld(asteroidGameObjectList, player, seed);

        Debug.Log(asteroidGameObjectList.Count);
        Debug.Log(Time.realtimeSinceStartup-startTime);

        ItemControl.selectedAsteroid = asteroidGameObjectList[0];
        ItemControl.AsteroidBlockControlScript = ItemControl.selectedAsteroid.GetComponent<AsteroidBlockControl>();
    }
}
