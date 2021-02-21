using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{
    // Strength of the bounce
    [SerializeField] float bounceStrength = 1.0f;

    // Possibility of activated or deactivated the platform, maybe for a puzzle?
    [SerializeField] bool isBouncy = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BouncePlayer(GameObject _player)
    {
        Vector3 force = new Vector3(0.0f, bounceStrength, 0.0f);
        _player.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isBouncy)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player");
                BouncePlayer(other.gameObject);
            }
        }
    }
}
