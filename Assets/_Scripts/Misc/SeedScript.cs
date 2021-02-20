/*  Author: Tyler McMillan
 *  Date Created: February 6, 2021
 *  Last Updated: February 6, 2021
 *  Description: This script is a trigger for the seed to be collected
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour
{
    private GameObject invScreen; //Reference to inventory gameobject/script
    public int seedWorth = 1; // how much is this seed worth.
    [SerializeField] string itemName;

    void Awake(){
        invScreen = GameObject.FindGameObjectWithTag("Inventory");
        itemName = gameObject.tag;
    }
    private void OnTriggerEnter(Collider col)
    {
        //When player steps in seeds trigger, add seed worth to players count and destroy seed.
        invScreen.GetComponent<PlayerInventory>().CollectSeed(seedWorth);
        if( invScreen.GetComponent<PlayerInventory>().CollectItem(itemName)){ //if item was collected destroy it
            Destroy(gameObject);
        }
    }
}
