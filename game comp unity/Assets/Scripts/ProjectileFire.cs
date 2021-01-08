using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFire : MonoBehaviour {

    public GameObject projectile;
    public Vector3 mousePos;
    public float projectileSpeed = 500f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P)) {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = Mathf.Atan2((mousePos.y - transform.position.y), (mousePos.x - transform.position.x)) * Mathf.Rad2Deg;
            Vector2 direction = (mousePos - transform.position).normalized;
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle));
            projectileInstance.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed);
            GetComponent<Rigidbody2D>().AddForce(-direction * projectileSpeed);
        }
	}

}
