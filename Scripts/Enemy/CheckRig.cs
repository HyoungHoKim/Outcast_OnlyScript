using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRig : MonoBehaviour
{
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
    if(other.gameObject.CompareTag("ENEMY"))
    {
        Debug.Log("트리거 충돌");
        
    }
    
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("ENEMY"))
        {
            Debug.Log("그냥 충돌");
        
        }
    }
        
}
