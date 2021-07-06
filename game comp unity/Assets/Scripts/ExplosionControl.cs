using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionControl : MonoBehaviour {

    public float explosionForce = 1f;
    public float timer = 0.0f;
    public bool destroyBlocks;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            Destroy(gameObject);
        }
	}
    void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb2d = collision.GetComponent<Rigidbody2D>();
        if (rb2d) {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            float distance = (transform.position - collision.transform.position).magnitude;
            rb2d.AddForce((-direction * explosionForce) / Mathf.Pow(distance, 2), ForceMode2D.Impulse);
            Debug.Log((-direction * explosionForce) / Mathf.Pow(distance, 2));
        }

    }
}
