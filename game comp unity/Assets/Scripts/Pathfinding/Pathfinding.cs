using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public int nodeWidth;
    public int nodeHeight;

    public Node[,] grid;

    public int startX;
    public int startY;

    public int nodeDistance = 1;

    public List<Vector2> path;
    public GameObject start;
    public GameObject end;

    public GameObject gridSquare;
    public GameObject pathSquare;
    public LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Node[nodeHeight, nodeWidth];
        for (int x = 0; x < nodeWidth; x++) {
            for (int y = 0; y < nodeHeight; y++) {
                Vector2 pos = new Vector2(startX + (x * nodeDistance), startY + (y * nodeDistance));
                grid[x, y] = new Node(pos);
            }
        }
        Vector2 startPosition = new Vector2(Mathf.RoundToInt(start.transform.position.x), Mathf.RoundToInt(start.transform.position.y));
        Vector2 endPosition = new Vector2(Mathf.RoundToInt(end.transform.position.x), Mathf.RoundToInt(end.transform.position.y));
        CreatePath(startPosition, endPosition);
        for (int i = 0; i < path.Count; i++) {
            Instantiate(pathSquare, path[i], Quaternion.identity);
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
        if (gridPosX < nodeWidth - 1) nodeList.Add(grid[gridPosX + 1, gridPosY]);
        if (gridPosX > 0) nodeList.Add(grid[gridPosX - 1, gridPosY]);
        if (gridPosY < nodeHeight - 1) nodeList.Add(grid[gridPosX, gridPosY + 1]);
        if (gridPosY > 0) nodeList.Add(grid[gridPosX, gridPosY - 1]);
        return nodeList;
    }

    bool IsWalkable(Node node) {

        return !Physics2D.OverlapCircle(node.position, 0.5f, mask);

    }



    void CreatePath(Vector2 start, Vector2 finish) {
        Node startNode = new Node(start);
        List<Node> openList = new List<Node>(){startNode};
        List<Node> closedList = new List<Node>();
        while (openList.Count > 0) {


            Node lowest = openList[0];
            for (int i = 1; i < openList.Count; i++) {
                if (openList[i].fCost < lowest.fCost) {
                    lowest = openList[i];
                }
            }
            
            if (lowest.position == finish) {
                path = new List<Vector2>();
                Node currentNode = lowest;
                while (currentNode.position != start) {
                    path.Add(currentNode.position);
                    currentNode = currentNode.parent;
                }
                path.Reverse();
                Debug.Log(path);
                return;
            }

            openList.Remove(lowest);
            closedList.Add(lowest);
            List<Node> adjacentNodes = GetAdjacentNodes(lowest);
            for (int i = 0; i < adjacentNodes.Count; i++) {
                if (IsWalkable(adjacentNodes[i]) && !closedList.Contains(adjacentNodes[i])) {

                    if (lowest.gCost + nodeDistance < adjacentNodes[i].gCost) {

                        adjacentNodes[i].parent = lowest;
                        adjacentNodes[i].gCost = lowest.gCost + nodeDistance;
                        adjacentNodes[i].hCost = Mathf.RoundToInt(Mathf.Abs(adjacentNodes[i].position.x - finish.x) + Mathf.Abs(adjacentNodes[i].position.y - finish.y));

                        if (!openList.Contains(adjacentNodes[i])) {
                            openList.Add(adjacentNodes[i]);
                        }
                    }
                }
            }
            

        }

    }
}