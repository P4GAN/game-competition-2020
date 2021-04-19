using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public int nodeWidth;
    public int nodeHeight;

    /*struct Node {
        public int positionX;
        public int positionY;
        public int gridX;
        public int gridY;
        public int parentX;
        public int parentY;
        public int gCost;
        public int fCost;

        public Node(Vector2 position) {
            this.positionX = position.x;
            this.positionY = position.y;
            this.
        }
    }*/

    public Node[,] grid;

    public int startX;
    public int startY;

    public int nodeDistance = 1;

    public LayerMask mask;
    public int maxIterations = 100000;

    // Start is called before the first frame update
    void Awake()
    {
        grid = new Node[nodeHeight, nodeWidth];
        for (int x = 0; x < nodeWidth; x++) {
            for (int y = 0; y < nodeHeight; y++) {
                Vector2 pos = new Vector2(startX + (x * nodeDistance), startY + (y * nodeDistance));
                grid[x, y] = new Node(pos);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<Node> GetAdjacentNodes(Node node) {
        int gridPosX = Mathf.RoundToInt((node.position.x - startX)/nodeDistance);
        int gridPosY = Mathf.RoundToInt((node.position.y - startY)/nodeDistance);
        List<Node> nodeList = new List<Node>();
        for (int x = gridPosX - 1; x < gridPosX + 2; x++) {
            for (int y = gridPosY - 1; y < gridPosY + 2; y++) {
                if (x < nodeWidth - 1 && x > 0 && y < nodeHeight - 1 && y > 0) {
                    nodeList.Add(grid[x, y]);
                }
            }
        }
        return nodeList;
    }

    bool IsWalkable(Node node) {

        return !Physics2D.OverlapCircle(node.position, 0.5f, mask);

    }



    public List<Vector2> CreatePath(Vector2 start, Vector2 finish) {
        
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
            
            if (lowest.position == finish) {
                List<Vector2> path = new List<Vector2>();
                Node currentNode = lowest;
                while (currentNode.position != start) {
                    path.Add(currentNode.position);
                    currentNode = currentNode.parent;
                }
                path.Reverse();


                return path;
            }

            openList.Remove(lowest);
            closedList.Add(lowest);
            List<Node> adjacentNodes = GetAdjacentNodes(lowest);
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
        return new List<Vector2>();

    }

    int GetDistance(Node a, Node b) {
		int distanceX = Mathf.RoundToInt(Mathf.Abs(a.position.x - b.position.x)/nodeDistance);
		int distanceY = Mathf.RoundToInt(Mathf.Abs(a.position.y - b.position.y)/nodeDistance);

		if (distanceX > distanceY)
			return 14 * distanceY + 10 * (distanceX - distanceY);
		return 14 * distanceX + 10 * (distanceY - distanceX);
    }
}