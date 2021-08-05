using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public GameObject laserProjectile;
    public float health = 1;
    public float moveSpeed = 3f;
    public Rigidbody2D rb2d;
    public float fireTimer;
    public float fireCooldown = 0.5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) {
            float angle = Mathf.Atan2((player.transform.position.y - transform.position.y), (player.transform.position.x - transform.position.x)) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (Vector2.Distance(player.transform.position, transform.position) >= 5f && player != null) {
            rb2d.velocity = transform.right * moveSpeed;
        }
        else {
            rb2d.velocity = new Vector2();
        }
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireCooldown && Vector2.Distance(player.transform.position, transform.position) <= 7.5f && player != null) {
            fireTimer = 0f;
            Vector2 direction = (player.transform.position - transform.position).normalized;
            ProjectileFire.DirectionFireProjectile(laserProjectile, direction, gameObject, 1000f);
        }
    }

    public bool TakeDamage(float damage) { // returns if the damage killed enemy or not
        health -= damage;
        if (health < 0) {
            player.GetComponent<AlienSpawn>().enemyList.Remove(gameObject);
            Destroy(gameObject);
            return true;
        }
        return false;
    }


}
