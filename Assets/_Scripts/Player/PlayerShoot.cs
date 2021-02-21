using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bullet;
    Animator anim;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Shoot");
            GameObject b = Instantiate(bullet);
            b.transform.position = transform.position;
            b.GetComponent<LazerScript>().direction = gameObject.transform.forward;
        }
    }
}
