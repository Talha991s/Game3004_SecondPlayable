using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnLogic : MonoBehaviour
{
    [SerializeField] GameObject currentSpawnPoint;
    GameObject oldSpawnPoint;
   // public PlayerHealth health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hazard"))
        {
            FindObjectOfType<SoundManager>().Play("Dying");   //this is the sound and was added by Salick for testing
            transform.position = currentSpawnPoint.transform.position;
            transform.rotation = currentSpawnPoint.transform.rotation;
        }

        if (other.gameObject.CompareTag("Spawn"))
        {
            oldSpawnPoint = currentSpawnPoint;
            currentSpawnPoint = other.gameObject;
            oldSpawnPoint = null;
        }
    }
}
