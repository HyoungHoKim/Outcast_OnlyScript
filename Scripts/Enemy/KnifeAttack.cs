using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class KnifeAttack : MonoBehaviour
{
    public GameObject knife;
    public Rigidbody knifeRig;
    public SteamVR_Behaviour_Pose pose;
    public Collider kinfeColl;
    public Transform stickPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        var velocity = pose.GetVelocity();
        if (velocity.magnitude > 1)
        {
            kinfeColl.isTrigger = true;
        }
        else
        {
            kinfeColl.isTrigger = false;
        }
        Debug.Log(velocity.magnitude);

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ENEMY"))
        {
            knifeRig.isKinematic = true;
            knife.transform.parent = other.transform;
            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ENEMY"))
        {
            knifeRig.isKinematic = true;
            knife.transform.parent = collision.transform;
          //  knife.transform.rotation = Quaternion.Euler(0, 0, 0);
          //  Vector3 offset = this.transform.position - stickPos.transform.position;
          //  knife.transform.position += offset;
            Debug.Log("박혔습니다");
        }
    }
    
    
}
