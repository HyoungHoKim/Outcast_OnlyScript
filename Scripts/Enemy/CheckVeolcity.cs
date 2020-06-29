using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CheckVeolcity : MonoBehaviour
{

    public SteamVR_Behaviour_Pose pose;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        var velocity = pose.GetVelocity();
    //    Debug.Log(velocity.magnitude);

    }
}
