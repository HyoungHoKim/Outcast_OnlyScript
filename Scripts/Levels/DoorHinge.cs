using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHinge : MonoBehaviour
{
    public Transform doorTr;
    public Rigidbody doorRig;

    void Start()
    {
        
    }

    void Update()
    {
        float doorRotY = doorTr.eulerAngles.y;
        print(doorRotY);
        if(doorRotY > 0f)
        {
            print("1");
            doorRotY = 0f;
        }
        /*if(doorRotY == 0f)
        {
            print("2");
            doorRig.velocity = Vector3.zero;
        }*/
    }
}
