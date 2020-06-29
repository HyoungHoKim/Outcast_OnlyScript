using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityCheck : MonoBehaviour
{

    Vector3 _myPriorPosition;
    Vector3 _myVelocityVector;
    public GameObject LHand;
    public GameObject RHand;

    


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        _myVelocityVector = (transform.position - _myPriorPosition) / Time.deltaTime;
        //   Debug.Log(_myVelocityVector.magnitude);
        _myPriorPosition = transform.position;

       

        /*
        if (_myVelocityVector.magnitude > 5 && LHand.active == false && RHand.active == false)
        {
            //
        }
        */
    }
}
