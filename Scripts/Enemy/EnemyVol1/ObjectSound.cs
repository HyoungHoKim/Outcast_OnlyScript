using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSound : MonoBehaviour
{

    public GameObject box1;
    public Rigidbody boxRig;
    public SphereCollider coll;

    public GameObject soundObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Instantiate(soundObj, transform);
        coll.radius = (boxRig.mass * boxRig.velocity.magnitude);
        Debug.Log(coll.radius + "소리다");
    }
}
