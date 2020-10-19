using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBlockControl : MonoBehaviour
{

    public Dictionary<Vector2, GameObject> asteroidBlocks = new Dictionary<Vector2, GameObject>{};
    //stores grid positions
    public BlockControl blockControlScript;

    // Start is called before the first frame update
    void Start()
    {
        blockControlScript = GameObject.Find("BlockControlGameObject").GetComponent<BlockControl>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    Vector2 gamePositionToGridPosition(Vector2 gamePosition) {


        gamePosition.x -= transform.position.x;
        gamePosition.y -= transform.position.y;

        float angle = Mathf.Atan2(gamePosition.y, gamePosition.x) - (Mathf.Deg2Rad * transform.eulerAngles.z);
        float distance = gamePosition.magnitude;
        gamePosition.x = Mathf.Cos(angle) * distance;
        gamePosition.y = Mathf.Sin(angle) * distance;

        gamePosition = new Vector2 (Mathf.Floor(gamePosition.x + 0.5f) , Mathf.Floor(gamePosition.y + 0.5f));

        return gamePosition;

    }
    Vector2 gridPositionToGamePosition(Vector2 gridPosition) {


        float angle = Mathf.Atan2(gridPosition.y, gridPosition.x) + (Mathf.Deg2Rad * transform.eulerAngles.z);
        float distance = gridPosition.magnitude;
        gridPosition.x = Mathf.Cos(angle) * distance;
        gridPosition.y = Mathf.Sin(angle) * distance;

        gridPosition.x += transform.position.x;
        gridPosition.y += transform.position.y;

        return gridPosition;

    }

    public bool IsOccupied(Vector2 position) {
        Vector2 gridPosition = gamePositionToGridPosition(position);
        foreach(Vector2 key in asteroidBlocks.Keys) {
            if (key == gridPosition) {
                return true;
            }
        }
        return false;
    }
    public GameObject PlaceBlock(int itemID, Vector2 position, bool adjacentPosition) {
        Vector2 gridPosition = gamePositionToGridPosition(position);
        blockControlScript = GameObject.Find("BlockControlGameObject").GetComponent<BlockControl>();
        if (!IsOccupied(position)) {
            if (adjacentPosition) {
                if (asteroidBlocks.ContainsKey(new Vector2(gridPosition.x + 1, gridPosition.y)) ||
                    asteroidBlocks.ContainsKey(new Vector2(gridPosition.x - 1, gridPosition.y)) ||
                    asteroidBlocks.ContainsKey(new Vector2(gridPosition.x, gridPosition.y + 1)) ||
                    asteroidBlocks.ContainsKey(new Vector2(gridPosition.x, gridPosition.y - 1))) {
                        GameObject blockInstance = Instantiate(blockControlScript.blockList[itemID], gridPositionToGamePosition(gridPosition), transform.rotation);
                        asteroidBlocks.Add(gridPosition, blockInstance);    
                        blockInstance.transform.parent = gameObject.transform;
                        return blockInstance;
                }
                else {return null;}
            }
            if (!adjacentPosition) {
                GameObject blockInstance = Instantiate(blockControlScript.blockList[itemID], gridPositionToGamePosition(gridPosition), transform.rotation);
                asteroidBlocks.Add(gridPosition, blockInstance);    
                blockInstance.transform.parent = gameObject.transform;
                return blockInstance;
            }
        }
        return null;
    }
    public GameObject RemoveBlock(Vector2 position ) {
        Vector2 gridPosition = gamePositionToGridPosition(position);
        if (IsOccupied(position)) {
            GameObject block = asteroidBlocks[gridPosition];
            Destroy(asteroidBlocks[gridPosition]);
            asteroidBlocks.Remove(gridPosition);
            return block;
        }
        return null;
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0) && blockControlScript.selectedAsteroid != gameObject) {
            Debug.Log(9);
            blockControlScript.selectedAsteroid = gameObject;
        }
        

    }
}
