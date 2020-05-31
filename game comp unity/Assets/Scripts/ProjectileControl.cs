using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControl : MonoBehaviour {

    public float timer = 0.0f;
    public GameObject explosion;
    public bool enemyProjectile;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player") && !enemyProjectile)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (enemyProjectile) {Destroy(gameObject);}
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1.5f)
        {
            Destroy(gameObject);
            if (!enemyProjectile) { Instantiate(explosion, transform.position, Quaternion.identity); }
        }

    }

}