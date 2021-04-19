using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControl : MonoBehaviour {

    public float timer = 0.0f;
    public GameObject explosion;
    public bool explosive = false;
    public string owner;
    public float damage;
    public float maxTime = 5f;

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("test");
        if (!col.CompareTag("Explosion")) {
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
            }
            if (!col.CompareTag("Enemy") && owner == "enemy" ) // enemy projectile
            {
                if (explosive) {
                    Instantiate(explosion, transform.position, Quaternion.identity);
                }
                if (col.CompareTag("Player")) {
                    col.GetComponent<PlayerHealth>().TakeDamage(damage);
                }
                Destroy(gameObject);
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