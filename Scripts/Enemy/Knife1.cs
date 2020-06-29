using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife1 : MonoBehaviour
{
    public Rigidbody knifeRig;
    public GameObject knife;
    public Animator ani;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ENEMY"))
        {
            this.transform.parent = collision.transform;
            Debug.Log("적과 충돌");
        }
        //knifeRig.isKinematic = true;
    }

   
        */
    //knifeRig.isKinematic = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ENEMY"))
        {
            this.transform.parent = other.transform;
            knifeRig.isKinematic = true;
            ani.enabled = false;
            Debug.Log("적과 충돌");
        }



    }
}
