using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] 
public class BlockObject {
    public float gridX;
    public float gridY;
    public int blockID;
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
    public List<AsteroidObject> asteroidList;
    public PlayerObject playerObject;
}

public class WorldBuilder : MonoBehaviour
{
    public GameObject asteroidGameObject;
    public List<GameObject> asteroidGameObjectList;
    public GameObject player;
    public GameObject playerPrefab;
    public ProceduralGeneration proceduralGenerationScript;

    void Awake() {
        if (File.Exists("world.json")) {
            LoadWorld("world.json");

        }
        else {
            GenerateWorld();
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            SaveWorld(asteroidGameObjectList, player);
        }
    }

    void SaveWorld(List<GameObject> AsteroidList, GameObject Player) {
        WorldObject worldObject = new WorldObject();
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

        foreach (AsteroidObject asteroidObject in worldObject.asteroidList) {
            GameObject asteroidGameObjectInstance = Instantiate(asteroidGameObject, new Vector2(asteroidObject.x, asteroidObject.y), Quaternion.Euler(0, 0, asteroidObject.angle));
            AsteroidBlockControl asteroidBlockControlScript = asteroidGameObjectInstance.GetComponent<AsteroidBlockControl>();
            foreach (BlockObject blockObject in asteroidObject.blockList) {
                GameObject blockObjectInstance = asteroidBlockControlScript.PlaceBlock(blockObject.blockID, asteroidBlockControlScript.gridPositionToGamePosition(new Vector2(blockObject.gridX, blockObject.gridY))); 
            }

            asteroidGameObjectInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(asteroidObject.velocityX, asteroidObject.velocityY);
            asteroidGameObjectInstance.GetComponent<Rigidbody2D>().angularVelocity = asteroidObject.angularVelocity;
            asteroidGameObjectList.Add(asteroidGameObjectInstance);
        }

        GameObject playerGameObjectInstance = Instantiate(playerPrefab, new Vector2(worldObject.playerObject.x, worldObject.playerObject.y), Quaternion.Euler(0, 0, worldObject.playerObject.angle));
        playerGameObjectInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(worldObject.playerObject.velocityX, worldObject.playerObject.velocityY);
        playerGameObjectInstance.GetComponent<Rigidbody2D>().angularVelocity = worldObject.playerObject.angularVelocity;
        playerGameObjectInstance.GetComponent<Inventory>().InventoryItems = worldObject.playerObject.InventoryItems;
        playerGameObjectInstance.name = "player";
        player = playerGameObjectInstance;
    }

    void GenerateWorld() {
        player = Instantiate(playerPrefab, new Vector2(0, 15), Quaternion.identity);
        player.name = "player";
        proceduralGenerationScript = GetComponent<ProceduralGeneration>();
        //asteroidGameObjectList = proceduralGenerationScript.RandomSpawning(64, 1024, 1024);
        asteroidGameObjectList.Add(proceduralGenerationScript.CreateAsteroid(0f, 0f, 10f, 5f));
        SaveWorld(asteroidGameObjectList, player);
    }
}
