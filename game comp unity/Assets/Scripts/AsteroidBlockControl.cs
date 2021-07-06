using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class AsteroidBlockControl : MonoBehaviour
{
    public float mass = 0;
    public Dictionary<Vector2, GameObject> asteroidBlocks = new Dictionary<Vector2, GameObject>{};
    public Rigidbody2D rb2d;
    public bool isShip;
    //stores grid positions

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2 gamePositionToGridPosition(Vector2 gamePosition) {

        gamePosition.x -= transform.position.x;
        gamePosition.y -= transform.position.y;

        float angle = Mathf.Atan2(gamePosition.y, gamePosition.x) - (Mathf.Deg2Rad * transform.eulerAngles.z);
        float distance = gamePosition.magnitude;
        gamePosition.x = Mathf.Cos(angle) * distance;
        gamePosition.y = Mathf.Sin(angle) * distance;

        gamePosition = new Vector2 (Mathf.Floor(gamePosition.x + 0.5f) , Mathf.Floor(gamePosition.y + 0.5f));

        return gamePosition;

    }
    public Vector2 gridPositionToGamePosition(Vector2 gridPosition) {


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
        return asteroidBlocks.ContainsKey(gridPosition);
    }
    
    public bool isAdjacent(Vector2 position) {
        Vector2 gridPosition = gamePositionToGridPosition(position);
        return (asteroidBlocks.ContainsKey(new Vector2(gridPosition.x + 1, gridPosition.y)) ||
                asteroidBlocks.ContainsKey(new Vector2(gridPosition.x - 1, gridPosition.y)) ||
                asteroidBlocks.ContainsKey(new Vector2(gridPosition.x, gridPosition.y + 1)) ||
                asteroidBlocks.ContainsKey(new Vector2(gridPosition.x, gridPosition.y - 1)));
    }

    public GameObject PlaceBlock(string itemID, Vector2 position, int rotation = 0) {
        Vector2 gridPosition = gamePositionToGridPosition(position);
        if (asteroidBlocks.ContainsKey(gridPosition)) {
            Destroy(asteroidBlocks[gridPosition]);
        }
        GameObject block = Instantiate(ItemControl.itemDictionary[itemID], gridPositionToGamePosition(gridPosition), transform.rotation);
        block.transform.parent = gameObject.transform;
        asteroidBlocks[gridPosition] = block;
        mass += block.GetComponent<ItemData>().item.mass;
        rb2d.mass = mass;
        block.transform.localRotation = Quaternion.Euler(0, 0, rotation);
        if (isShip) {
            GetComponent<ShipControl>().AddShipBlock(block);
        }
        return block;
    }
    public GameObject PlaceBlockPlayer(string itemID, Vector2 position, bool adjacentPosition, int rotation) {
        Vector2 gridPosition = gamePositionToGridPosition(position);

        if (!IsOccupied(position)) {
            if (adjacentPosition) {
                if (isAdjacent(position)) {
                    return PlaceBlock(itemID, position, rotation);
                }
                else {
                    return null;
                }
            }
            if (!adjacentPosition) {
                return PlaceBlock(itemID, position, rotation);         
            }
        }
        return null;
    }
    public GameObject RemoveBlock(Vector2 position ) {
        Vector2 gridPosition = gamePositionToGridPosition(position);
        if (IsOccupied(position)) {
            GameObject block = asteroidBlocks[gridPosition];
            mass -= block.GetComponent<ItemData>().item.mass;
            rb2d.mass = mass;
            Destroy(asteroidBlocks[gridPosition]);
            asteroidBlocks.Remove(gridPosition);

            Profiler.BeginSample("expensive");

            FindAsteroidPieces();

            Profiler.EndSample();
            if (isShip) {
                GetComponent<ShipControl>().AddShipBlock(block);
            }
            return block;
        }
        return null;
    }

    //get the broken off bits of the asteroid
    public List<GameObject> FindAsteroidPieces() {
        Debug.Log("REEEEEE");
        HashSet<Vector2> markedPositions = new HashSet<Vector2>();
        List<GameObject> asteroidGameObjects = new List<GameObject>();

        //for each block in the asteroid if it isnt marked, perform a flood fill and mark filled blocks
        foreach (Vector2 asteroidBlockPosition in asteroidBlocks.Keys) {
            if (!markedPositions.Contains(asteroidBlockPosition)) {
                List<Vector2> outerLayer = new List<Vector2>(){ asteroidBlockPosition };
                List<Vector2> asteroidPiecePositions  = new List<Vector2>();
                while (outerLayer.Count != 0) {
                    Vector2 position = outerLayer[0];
                    outerLayer.RemoveAt(0);
                    //if the position is in the asteroid, and it isnt marked
                    if (asteroidBlocks.ContainsKey(position) && !markedPositions.Contains(position)) {
                        markedPositions.Add(position);
                        asteroidPiecePositions.Add(position);

                        outerLayer.Add(new Vector2(position.x + 1, position.y));
                        outerLayer.Add(new Vector2(position.x - 1, position.y));
                        outerLayer.Add(new Vector2(position.x, position.y + 1));
                        outerLayer.Add(new Vector2(position.x, position.y - 1));
                    }
                        
                }

                //if there is more than one piece, create gameobjects, otherwise continue
                //check by seeing if it is not first gameobject, or if not all blocks are marked
                Debug.Log("one");
                Debug.Log(markedPositions.Count);
                Debug.Log(asteroidBlocks.Count);
                Debug.Log(asteroidGameObjects.Count);
                Debug.Log("hello?");
                Debug.Log(asteroidGameObjects);

                if (markedPositions.Count == asteroidBlocks.Count && asteroidGameObjects.Count < 1) {
                    Debug.Log("test");
                    asteroidGameObjects.Add(gameObject);
                    
                }

                else {
                    Debug.Log(markedPositions.Count == asteroidBlocks.Count);
                    GameObject asteroidGameObjectInstance = Instantiate(WorldBuilder.asteroidGameObject, transform.position, Quaternion.identity);

                    asteroidGameObjectInstance.transform.rotation = transform.rotation;

                    AsteroidBlockControl asteroidBlockControlScript = asteroidGameObjectInstance.GetComponent<AsteroidBlockControl>();
                    for (int i = 0; i < asteroidPiecePositions.Count; i++) {
                        asteroidBlockControlScript.PlaceBlock(asteroidBlocks[asteroidPiecePositions[i]].GetComponent<ItemData>().item.itemID, gridPositionToGamePosition(asteroidPiecePositions[i]));

                    }
                    asteroidGameObjects.Add(asteroidGameObjectInstance);
                }



            }

        }
        if (asteroidGameObjects.Count > 1) {
            WorldBuilder.asteroidGameObjectList.Remove(gameObject);
            WorldBuilder.asteroidGameObjectList.AddRange(asteroidGameObjects);
            Destroy(gameObject);
        }

        return asteroidGameObjects;
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0) && ItemControl.selectedAsteroid != gameObject) {
            ItemControl.selectedAsteroid = gameObject;
            ItemControl.AsteroidBlockControlScript = this;
        }
        

    }
}
