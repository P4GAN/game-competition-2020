using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public int chunkSize = 100;


    public Node[,] grid;

    public Vector2 center = new Vector2();

    public int nodeSeparatingDistance = 1;

    public LayerMask mask;
    public int maxIterations = 100;

    public Vector2 playerChunkPosition = new Vector2();

    // Start is called before the first frame update
    void Awake()
    {
        grid = new Node[chunkSize * 3, chunkSize * 3];
        for (int x = -chunkSize * 3 / 2; x < chunkSize * 3 / 2; x++) {
            for (int y = -chunkSize * 3 / 2; y < chunkSize * 3 / 2; y++) {
                Vector2 pos = new Vector2(center.x + (x * nodeSeparatingDistance), center.y + (y * nodeSeparatingDistance));
                grid[x + chunkSize * 3 / 2, y + chunkSize * 3 / 2] = new Node(pos);
            }
        }
    }


    
    void Update()
    {
        Vector2 newPlayerChunkPosition = new Vector2();
        newPlayerChunkPosition.x = Mathf.Floor((WorldBuilder.player.transform.position.x + (chunkSize * nodeSeparatingDistance / 2))/(chunkSize * nodeSeparatingDistance));
        newPlayerChunkPosition.y = Mathf.Floor((WorldBuilder.player.transform.position.y + (chunkSize * nodeSeparatingDistance / 2))/(chunkSize * nodeSeparatingDistance));


        if (newPlayerChunkPosition != playerChunkPosition) {
            playerChunkPosition = newPlayerChunkPosition;
            center.x = newPlayerChunkPosition.x * (chunkSize * nodeSeparatingDistance);
            center.y = newPlayerChunkPosition.y * (chunkSize * nodeSeparatingDistance);
            
            grid = new Node[chunkSize, chunkSize];
            for (int x = -chunkSize * 3 / 2; x < chunkSize * 3 / 2; x++) {
                for (int y = -chunkSize * 3 / 2; y < chunkSize * 3 / 2; y++) {
                    Vector2 pos = new Vector2(center.x + (x * nodeSeparatingDistance), center.y + (y * nodeSeparatingDistance));
                    grid[x + chunkSize * 3 / 2, y + chunkSize * 3 / 2] = new Node(pos);
                }
                
            }

        }


        
    }

    List<Node> GetAdjacentNodes(Node node) {
        return new List<Node>(){
            new Node(node.worldPosition + new Vector2(-nodeSeparatingDistance, -nodeSeparatingDistance)),
            new Node(node.worldPosition + new Vector2(-nodeSeparatingDistance, 0)),
            new Node(node.worldPosition + new Vector2(-nodeSeparatingDistance, nodeSeparatingDistance)),
            new Node(node.worldPosition + new Vector2(0, -nodeSeparatingDistance)),
            new Node(node.worldPosition + new Vector2(0, nodeSeparatingDistance)),
            new Node(node.worldPosition + new Vector2(nodeSeparatingDistance, -nodeSeparatingDistance)),
            new Node(node.worldPosition + new Vector2(nodeSeparatingDistance, 0)),
            new Node(node.worldPosition + new Vector2(nodeSeparatingDistance, nodeSeparatingDistance)),
        };
    }

    bool IsWalkable(Node node) {

        return !Physics2D.OverlapCircle(node.worldPosition, nodeSeparatingDistance/2, mask);

    }



    public List<Vector2> CreatePath(Vector2 start, Vector2 finish) {
        Debug.Log("j");
        Node startNode = new Node(start);
        Node finishNode = new Node(finish);
        List<Node> openList = new List<Node>(){startNode};
        List<Node> closedList = new List<Node>();
        int iterations = 0;
        while (openList.Count > 0) {

            iterations += 1;
            if (iterations >= maxIterations) {
                break;
            }
            Node lowest = openList[0];
            for (int i = 1; i < openList.Count; i++) {
                if (openList[i].fCost <= lowest.fCost && openList[i].hCost < lowest.hCost) {
                    lowest = openList[i];
                }
            }
            
            if (lowest.worldPosition == finish) {
                List<Vector2> path = new List<Vector2>();
                Node currentNode = lowest;
                while (currentNode.worldPosition != start) {
                    path.Add(currentNode.worldPosition);
                    currentNode = currentNode.parent;
                }
                path.Reverse();
                Debug.Log("S");

                return path;
            }

            openList.Remove(lowest);
            closedList.Add(lowest);
            List<Node> adjacentNodes = GetAdjacentNodes(lowest);
            Debug.Log(adjacentNodes.Count);
            for (int i = 0; i < adjacentNodes.Count; i++) {
                if (IsWalkable(adjacentNodes[i]) && !closedList.Contains(adjacentNodes[i])) {
                    if (lowest.gCost + GetDistance(lowest, adjacentNodes[i]) < adjacentNodes[i].gCost || !openList.Contains(adjacentNodes[i])) {

                        adjacentNodes[i].parent = lowest;
                        adjacentNodes[i].gCost = lowest.gCost + GetDistance(lowest, adjacentNodes[i]);
                        adjacentNodes[i].hCost = GetDistance(adjacentNodes[i], finishNode);

                        if (!openList.Contains(adjacentNodes[i])) {
                            openList.Add(adjacentNodes[i]);
                        }
                    }
                }
            }
            

        }
        Debug.Log("non");
        return new List<Vector2>();

    }

    int GetDistance(Node a, Node b) {
		int distanceX = Mathf.RoundToInt(Mathf.Abs(a.worldPosition.x - b.worldPosition.x)/nodeSeparatingDistance);
		int distanceY = Mathf.RoundToInt(Mathf.Abs(a.worldPosition.y - b.worldPosition.y)/nodeSeparatingDistance);

		if (distanceX > distanceY)
			return 14 * distanceY + 10 * (distanceX - distanceY);
		return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}