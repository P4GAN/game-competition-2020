using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFire : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void MouseFireProjectile(GameObject projectile, GameObject startObject, float projectileSpeed = 1000f) {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2((mousePos.y - startObject.transform.position.y), (mousePos.x - startObject.transform.position.x)) * Mathf.Rad2Deg;
        Vector2 direction = (mousePos - startObject.transform.position).normalized;
        GameObject projectileInstance = Instantiate(projectile, startObject.transform.position, Quaternion.Euler(0, 0, angle));
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed);
        //startObject.GetComponent<Rigidbody2D>().AddForce(-direction * projectileSpeed);
    }

    public void DirectionFireProjectile(GameObject projectile, Vector2 direction, GameObject startObject, float projectileSpeed = 1000f) {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject projectileInstance = Instantiate(projectile, startObject.transform.position, Quaternion.Euler(0, 0, angle));
        projectileInstance.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed);
        //startObject.GetComponent<Rigidbody2D>().AddForce(-direction * projectileSpeed);
    }

}
