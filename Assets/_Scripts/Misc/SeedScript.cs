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

    Vector3 startingPosition;

    void Awake()
    {
        invScreen = GameObject.FindGameObjectWithTag("Inventory");
        itemName = gameObject.tag;
    }

    void Start()
    {
        startingPosition = transform.position;
    }
    void Update()
    {
        Hover();
        Rotate();
    }
    void Hover()
    {
        transform.position = Vector3.Lerp(startingPosition, startingPosition + new Vector3(0.0f, 0.1f, 0.0f), Mathf.PingPong(Time.time * 2, 1.0f));
    }

    void Rotate()
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f));
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
