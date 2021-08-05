using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBanditController : MonoBehaviour
{
    // Start is called before the first frame update
    public string state = "roaming";
    public Vector2 roamPosition;
    public EnemyPathfinding enemyPathfindingScript;
    public LayerMask mask;
    public GameObject player;
    public float chaseDistance;
    public float attackDistance;
    public GameObject projectile;
    public float shootCooldown;
    public float shootTimer;

    void Start()
    {
        enemyPathfindingScript = GetComponent<EnemyPathfinding>();
        roamPosition = RandomPosition();
        enemyPathfindingScript.SetTargetPosition(roamPosition);
        player = WorldBuilder.player;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (state == "roaming") {
            enemyPathfindingScript.SetTargetPosition(roamPosition);
            if (Mathf.Abs(roamPosition.x - transform.position.x) < 0.5f && Mathf.Abs(roamPosition.y - transform.position.y) < 0.5f) {
                roamPosition = RandomPosition();
                Debug.Log("Roam position is " + roamPosition);
            } 
            if (Vector2.Distance(player.transform.position, transform.position) <= chaseDistance) {
                state = "chase";
            } 
        }
        if (state == "chase") {
            enemyPathfindingScript.SetTargetPosition(player.transform.position);
            if (Vector2.Distance(player.transform.position, transform.position) > chaseDistance) {
                state = "roaming";
            }
            if (Vector2.Distance(player.transform.position, transform.position) <= attackDistance) {
                state = "attack";
            }
        }
        if (state == "attack") {
            if (Vector2.Distance(player.transform.position, transform.position) > attackDistance) {
                state = "chase";
            }
            if (shootTimer >= shootCooldown) {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                ProjectileFire.DirectionFireProjectile(projectile, direction, gameObject);
                shootTimer = 0f;
            }
        }

        
    }

    public Vector2 RandomPosition() {
        int iterations = 0;
        while (true) {
            iterations += 1;
            if (iterations > 100000) {
                return transform.position;
            }
            Vector3 position = new Vector3(Random.Range(10, 90), Random.Range(10, 90), 0f);
            if (!Physics2D.OverlapBox(position, new Vector2(1, 1), 0, mask)) {
                return position;
            }
        }
    }
}
