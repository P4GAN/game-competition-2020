using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Vector2> pathVectorList;
    public int pathIndex = 0;
    public float pathfindingTimer = 0f;
    public float pathfindingInterval = 0.5f;

    public GameObject pathfindingGameObject;
    public Pathfinding pathfindingScript;
    public Vector2 moveDirection;
    public Rigidbody2D rb2d;
    public float speed = 20f;

    void Start()
    {
        pathfindingScript = pathfindingGameObject.GetComponent<Pathfinding>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        pathfindingTimer -= Time.deltaTime;

        if (pathVectorList.Count > 0) {
            Vector3 targetPosition = pathVectorList[pathIndex];
            if (Mathf.Abs(targetPosition.x - transform.position.x) < 0.5f && Mathf.Abs(targetPosition.y - transform.position.y) < 0.5f) {
                pathIndex += 1;
                if (pathIndex >= pathVectorList.Count) {
                    pathVectorList = new List<Vector2>();
                    moveDirection = new Vector2();
                }
                else {
                    targetPosition = pathVectorList[pathIndex];
                    moveDirection = (targetPosition - transform.position).normalized;
                }
            }
            else {
                moveDirection = (targetPosition - transform.position).normalized;
            }
            for (int i = 0; i < pathVectorList.Count - 1; i++) {
                Debug.DrawLine(pathVectorList[i], pathVectorList[i + 1]);
            }
        }
    }

    void FixedUpdate() {
        rb2d.velocity = moveDirection * speed;
    }

    public void SetTargetPosition(Vector2 position) 
    {
        if (pathfindingTimer <= 0) {
            pathfindingTimer = pathfindingInterval;
            pathIndex = 0;
            Vector2 startPosition = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            Vector2 endPosition = new Vector2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
            float startTime = Time.realtimeSinceStartup;
            pathVectorList = pathfindingScript.CreatePath(startPosition, endPosition);
            Debug.Log(Time.realtimeSinceStartup-startTime);

        }
    }
}
