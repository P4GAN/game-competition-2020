using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResources : MonoBehaviour
{
    // Start is called before the first frame update

    public float health = 100f;
    public float maxHealth = 100f;
    public float oxygen = 100f;
    public float maxOxygen = 100f;
    public float power = 0f;
    public float maxPower = 100f;
    public float oxygenUsage = 1f;
    public float oxygenDamage = 1f;

    public Slider healthSlider;
    public Slider powerSlider;

    void Start()
    {
        healthSlider = SceneReferences.healthSlider;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        oxygen -= oxygenUsage;
        if (oxygen <= 0) {
            oxygen = 0;
            //TakeDamage(oxygenDamage);
        }
        if (oxygen >= maxOxygen) {
            oxygen = maxOxygen;
        }
        if (health >= maxHealth) {
            health = maxHealth;
        }
        if (power >= maxHealth) {
            power = maxPower;
        }
        if (power <= 0) {
            power = 0;
        }
        healthSlider.value = health;
    }

    public bool TakeDamage(float damage) {
        health -= damage;
        if (health < 0) {
            Debug.Log("dead L");
            return true;
        }
        return false;
    }
}
