/*  Author: Salick Talhah
 *  Date Created: February 14, 2021
 *  Last Updated: February 14, 2021
 *  Description: This script is used to control the damage and amount of health, load the game over screen and check collision with hazard.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject gameover;
    public int maxhealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxhealth;
        healthBar.SetMaxHealth(maxhealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth == 0)
        {
            gameover.SetActive(true);
        }
    }
   public void TakeDamage(int damage)
   {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
   }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hazard"))
        {
            TakeDamage(20);
        }

    }
}
