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
    public List<BlockObject> blockList;

}
[Serializable]
public class PlayerObject {
    public float x;
    public float y;
    public float angle;
    public float velocityX;
    public float velocityY;
    public List<int> InventoryItems;
    public List<int> InventoryAmounts;
}


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
    public ItemControl ItemControlScript;

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
            asteroidObject.blockList = new List<BlockObject>();

            foreach (Vector2 position in asteroid.GetComponent<AsteroidBlockControl>().asteroidBlocks.Keys) {

                BlockObject blockObject = new BlockObject();
                blockObject.gridX = position.x;
                blockObject.gridY = position.y;
                blockObject.blockID = asteroid.GetComponent<AsteroidBlockControl>().asteroidBlocks[position].GetComponent<ItemData>().itemID;
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
        playerObject.InventoryItems = Player.GetComponent<Inventory>().InventoryItems;
        playerObject.InventoryAmounts = Player.GetComponent<Inventory>().InventoryAmounts;

        worldObject.playerObject = playerObject;

        string json = JsonUtility.ToJson(worldObject, true);
        File.WriteAllText("world.json", json);
    }


    void LoadWorld(string fileName) {
        string json = File.ReadAllText(fileName);
        WorldObject worldObject = JsonUtility.FromJson<WorldObject>(json);
        ItemControlScript = GameObject.Find("ItemControlGameObject").GetComponent<ItemControl>();

        foreach (AsteroidObject asteroidObject in worldObject.asteroidList) {
            GameObject asteroidGameObjectInstance = Instantiate(asteroidGameObject, new Vector2(asteroidObject.x, asteroidObject.y), Quaternion.Euler(0, 0, asteroidObject.angle));
            
            foreach (BlockObject blockObject in asteroidObject.blockList) {
                Vector2 position = asteroidGameObjectInstance.GetComponent<AsteroidBlockControl>().gridPositionToGamePosition(new Vector2(blockObject.gridX, blockObject.gridY));
                GameObject blockObjectInstance = Instantiate(ItemControlScript.itemList[blockObject.blockID], position, Quaternion.Euler(0, 0, asteroidObject.angle)); 
                blockObjectInstance.transform.SetParent(asteroidGameObjectInstance.transform);
            }

            asteroidGameObjectInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(asteroidObject.velocityX, asteroidObject.velocityY);

            asteroidGameObjectList.Add(asteroidGameObjectInstance);
        }

        GameObject playerGameObjectInstance = Instantiate(playerPrefab, new Vector2(worldObject.playerObject.x, worldObject.playerObject.y), Quaternion.Euler(0, 0, worldObject.playerObject.angle));
        playerGameObjectInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(worldObject.playerObject.velocityX, worldObject.playerObject.velocityY);
        playerGameObjectInstance.GetComponent<Inventory>().InventoryItems = worldObject.playerObject.InventoryItems;
        playerGameObjectInstance.GetComponent<Inventory>().InventoryAmounts = worldObject.playerObject.InventoryAmounts;
        playerGameObjectInstance.name = "player";
        player = playerGameObjectInstance;
    }

    void GenerateWorld() {
        player = Instantiate(playerPrefab, new Vector2(0, 15), Quaternion.identity);
        player.name = "player";
        proceduralGenerationScript = GetComponent<ProceduralGeneration>();
        asteroidGameObjectList.Add(proceduralGenerationScript.CreateAsteroid(0f, 0f, 10f, 5f));
        SaveWorld(asteroidGameObjectList, player);
    }
}
