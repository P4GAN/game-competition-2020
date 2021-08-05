using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;


public class AsteroidBlockControl : MonoBehaviour
{
    
    public float mass = 0;
    public GameObject asteroidMinimapIndicator;
    public Dictionary<Vector2, string> asteroidBlocks = new Dictionary<Vector2, string>{};
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public PolygonCollider2D polygonCollider2d;
    public Rigidbody2D rb2d;
    public HashSet<Edge> meshEdges;

    public float blockSize = 1;
    public bool isShip;
    private List<Vector2> directions = new List<Vector2>{new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0)};
    //stores grid positions

    // Start is called before the first frame update
    void Awake()
    {
        meshRenderer.material = SpriteSheetCreator.spriteSheetMaterial;
    }
    public void GenerateMesh() {

        asteroidMinimapIndicator.transform.localScale = new Vector3(Mathf.Sqrt(rb2d.mass)/10, Mathf.Sqrt(rb2d.mass)/10, 0);

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4 * asteroidBlocks.Count];
        int[] triangles = new int[6 * asteroidBlocks.Count];
        Vector2[] uv = new Vector2[4 * asteroidBlocks.Count];

        int verticesCount = 0;

        meshEdges = new HashSet<Edge>();


        foreach (Vector2 position in asteroidBlocks.Keys) {
            vertices[verticesCount] = position + new Vector2(-blockSize/2, -blockSize/2);
            vertices[verticesCount + 1] = position + new Vector2(blockSize/2, -blockSize/2);
            vertices[verticesCount + 2] = position + new Vector2(-blockSize/2, blockSize/2);
            vertices[verticesCount + 3] = position + new Vector2(blockSize/2, blockSize/2);

            //index is number of vertices / 4 then multiplied by 6
            triangles[verticesCount * 3 / 2] = verticesCount;
            triangles[verticesCount * 3 / 2 + 1] = verticesCount + 2;
            triangles[verticesCount * 3 / 2 + 2] = verticesCount + 1;
            triangles[verticesCount * 3 / 2 + 3] = verticesCount + 2;
            triangles[verticesCount * 3 / 2 + 4] = verticesCount + 3;
            triangles[verticesCount * 3 / 2 + 5] = verticesCount + 1;

            Edge edge1 = new Edge{a = vertices[verticesCount], b = vertices[verticesCount + 1]};
            Edge edge2 = new Edge{a = vertices[verticesCount], b = vertices[verticesCount + 2]};
            Edge edge3 = new Edge{a = vertices[verticesCount + 1], b = vertices[verticesCount + 3]};
            Edge edge4 = new Edge{a = vertices[verticesCount + 2], b = vertices[verticesCount + 3]};

            if (meshEdges.Contains(edge1)) { meshEdges.Remove(edge1); } else { meshEdges.Add(edge1); }
            if (meshEdges.Contains(edge2)) { meshEdges.Remove(edge2); } else { meshEdges.Add(edge2); }
            if (meshEdges.Contains(edge3)) { meshEdges.Remove(edge3); } else { meshEdges.Add(edge3); }
            if (meshEdges.Contains(edge4)) { meshEdges.Remove(edge4); } else { meshEdges.Add(edge4); }


            Rect blockUV = SpriteSheetCreator.blockUV[asteroidBlocks[position]];

            uv[verticesCount] = new Vector2(blockUV.x, blockUV.y);
            uv[verticesCount + 1] = new Vector2(blockUV.x + blockUV.width, blockUV.y);
            uv[verticesCount + 2] = new Vector2(blockUV.x, blockUV.y + blockUV.height);
            uv[verticesCount + 3] = new Vector2(blockUV.x + blockUV.width, blockUV.y + blockUV.height);

            verticesCount += 4;
        }

        List<Vector2[]> pathList = CreatePathsFromEdges(meshEdges);
        polygonCollider2d.pathCount = pathList.Count;

        for (int i = 0; i < pathList.Count; i++) {
            polygonCollider2d.SetPath(i, pathList[i]);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    public List<Vector2[]> CreatePathsFromEdges(HashSet<Edge> edges) {
        List<Vector2[]> colliderPaths = new List<Vector2[]>();
        while (edges.Count > 0) {
            Edge startEdge = edges.ToList()[0];
            Vector2 startPoint = startEdge.a;

            edges.Remove(startEdge);

            Edge currentEdge = startEdge;
            Vector2 currentPoint = startEdge.b;

            List<Vector2> path = new List<Vector2>() {startPoint, currentPoint};
            while (currentPoint != startPoint) {
                for (int i = 0; i < directions.Count; i++) {
                    if (currentPoint + directions[i] != currentEdge.a || currentPoint + directions[i] != currentEdge.b) {
                        Edge testEdge = new Edge{a = currentPoint, b = currentPoint + directions[i]};

                        if (edges.Contains(testEdge)) {
                            edges.Remove(testEdge);
                            path.Add(currentPoint + directions[i]);
                            currentPoint = currentPoint + directions[i];
                            currentEdge = testEdge;
                            break;
                        }
                    }
                }

            }

            colliderPaths.Add(path.ToArray());
        }
        return colliderPaths;
    }

    public Vector2 GamePositionToGridPosition(Vector2 gamePosition) {

        gamePosition.x -= transform.position.x;
        gamePosition.y -= transform.position.y;

        float angle = Mathf.Atan2(gamePosition.y, gamePosition.x) - (Mathf.Deg2Rad * transform.eulerAngles.z);
        float distance = gamePosition.magnitude;
        gamePosition.x = Mathf.Cos(angle) * distance;
        gamePosition.y = Mathf.Sin(angle) * distance;

        gamePosition = new Vector2 (Mathf.Floor(gamePosition.x + blockSize/2) , Mathf.Floor(gamePosition.y + blockSize/2));

        return gamePosition;

    }
    public Vector2 GridPositionToGamePosition(Vector2 gridPosition) {

        float angle = Mathf.Atan2(gridPosition.y, gridPosition.x) + (Mathf.Deg2Rad * transform.eulerAngles.z);
        float distance = gridPosition.magnitude;
        gridPosition.x = Mathf.Cos(angle) * distance;
        gridPosition.y = Mathf.Sin(angle) * distance;

        gridPosition.x += transform.position.x;
        gridPosition.y += transform.position.y;

        return gridPosition;

    }

    public bool IsOccupied(Vector2 gamePosition) {
        Vector2 gridPosition = GamePositionToGridPosition(gamePosition);
        return asteroidBlocks.ContainsKey(gridPosition);
    }
    
    public bool IsAdjacent(Vector2 gamePosition) {
        Vector2 gridPosition = GamePositionToGridPosition(gamePosition);
        return (asteroidBlocks.ContainsKey(new Vector2(gridPosition.x + 1, gridPosition.y)) ||
                asteroidBlocks.ContainsKey(new Vector2(gridPosition.x - 1, gridPosition.y)) ||
                asteroidBlocks.ContainsKey(new Vector2(gridPosition.x, gridPosition.y + 1)) ||
                asteroidBlocks.ContainsKey(new Vector2(gridPosition.x, gridPosition.y - 1)));
    }

    public string PlaceBlock(string itemID, Vector2 gamePosition, int rotation = 0) {
        Vector2 gridPosition = GamePositionToGridPosition(gamePosition);
        if (asteroidBlocks.ContainsKey(gridPosition)) {
            asteroidBlocks.Remove(gridPosition);
        }
        asteroidBlocks[gridPosition] = itemID;

        mass += 1;
        rb2d.mass = mass;

        /*

        /*mass += block.GetComponent<ItemData>().item.mass;
        rb2d.mass = mass;
        block.transform.localRotation = Quaternion.Euler(0, 0, rotation);
        if (isShip) {
            GetComponent<ShipControl>().AddShipBlock(block);
        }*/
        return itemID;
    }
    public string PlaceBlockPlayer(string itemID, Vector2 gamePosition, bool adjacentPosition, int rotation) {
        Vector2 gridPosition = GamePositionToGridPosition(gamePosition);

        if (!IsOccupied(gamePosition)) {
            if (adjacentPosition) {
                if (IsAdjacent(gamePosition)) {
                    return PlaceBlock(itemID, gamePosition, rotation);
                }
                else {
                    return null;
                }
            }
            if (!adjacentPosition) {
                return PlaceBlock(itemID, gamePosition, rotation);         
            }
        }
        return null;
    }
    public string RemoveBlock(Vector2 gamePosition ) {
        Vector2 gridPosition = GamePositionToGridPosition(gamePosition);
        if (IsOccupied(gamePosition)) {
            //GameObject block = asteroidBlocks[gridPosition];
            //mass -= block.GetComponent<ItemData>().item.mass;
            mass -= 1;
            rb2d.mass = mass;
            string block = asteroidBlocks[gridPosition];
            asteroidBlocks.Remove(gridPosition);

            Profiler.BeginSample("expensive");

            FindAsteroidPieces();

            Profiler.EndSample();
            /*if (isShip) {
                GetComponent<ShipControl>().RemoveShipBlock(block);
            }*/
            return block;
        }
        return "";
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
                    asteroidGameObjectInstance.GetComponent<Rigidbody2D>().velocity = rb2d.velocity;

                    AsteroidBlockControl asteroidBlockControlScript = asteroidGameObjectInstance.GetComponent<AsteroidBlockControl>();
                    for (int i = 0; i < asteroidPiecePositions.Count; i++) {
                        asteroidBlockControlScript.PlaceBlock(asteroidBlocks[asteroidPiecePositions[i]], GridPositionToGamePosition(asteroidPiecePositions[i]));

                    }
                    asteroidBlockControlScript.GenerateMesh();
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
public struct Edge {
    public Vector2 a;
    public Vector2 b;
    public override bool Equals (object obj)
    {
        if (obj is Edge) {
            var edge = (Edge)obj;
            //An edge is equal regardless of which order it's points are in
            return (edge.a == a && edge.b == b) || (edge.b == a && edge.a == b);
        }
        return false;
    }

    public override int GetHashCode ()
    {
        return a.GetHashCode() ^ b.GetHashCode();
    }
    
    public override string ToString ()
    {
        return string.Format ("["+a.x+","+a.y+"->"+b.x+","+b.y+"]");
    }
     
}
