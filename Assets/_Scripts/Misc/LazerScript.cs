using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerScript : MonoBehaviour
{
    public GameObject target;
    public float angle;
    public float speed;

    Collider col;
    public MeshRenderer mesh;
    public TrailRenderer trail;
    public Color32 color;
    Vector3 direction;
    
    void Start()
    {
        col = GetComponent<Collider>();
        mesh = GetComponent<MeshRenderer>();
        trail = GetComponentInChildren<TrailRenderer>();
        trail.endColor = new Color32(0, 0, 0, 0);
        mesh.material.color = color;
        trail.startColor = color;
        direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        transform.position += Vector3.up;
    }

    
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }

}
