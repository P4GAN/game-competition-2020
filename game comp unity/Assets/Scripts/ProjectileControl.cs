using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControl : MonoBehaviour {

    public float timer = 0.0f;
    public GameObject explosion;
    public bool explosive = false;
    public bool destroyBlocks = true;
    public string owner;
    public float damage;
    public float maxTime = 5f;
    public GameObject player;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Explosion")) {
            if (col.CompareTag("Asteroid")) {
                AsteroidBlockControl asteroidBlockControlScript = col.GetComponent<AsteroidBlockControl>();
                string block = asteroidBlockControlScript.RemoveBlock(transform.position);
                asteroidBlockControlScript.GenerateMesh();
                
                if (owner == "player" && player != null) {
                    player.GetComponent<SpacePlayerController>().AddItem(block, 1);
                }

                Destroy(gameObject);
                return;

            }
            if (!col.CompareTag("Player") && owner == "player") // player projectile
            {
                Debug.Log("hmm");
                if (explosive) {
                    Instantiate(explosion, transform.position, Quaternion.identity);
                }
                if (col.CompareTag("Enemy")) {
                    Debug.Log(damage);
                    col.GetComponent<Enemy>().TakeDamage(damage);
                }
                Destroy(gameObject);
                return;
            }
            if (!col.CompareTag("Enemy") && owner == "enemy" ) // enemy projectile
            {
                if (explosive) {
                    Instantiate(explosion, transform.position, Quaternion.identity);
                }
                if (col.CompareTag("Player")) {
                    col.GetComponent<SpacePlayerController>().TakeDamage(damage);
                }
                Destroy(gameObject);
                return;
            }
        }
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= maxTime)
        {
            Destroy(gameObject);
            if (explosive) { Instantiate(explosion, transform.position, Quaternion.identity); }
        }

    }

}