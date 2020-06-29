using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
//using System.Security.Policy;
using UnityEngine;
using UnityEngine.UIElements;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class MyJump : MonoBehaviour
{
    public SteamVR_Action_Boolean JumpAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");

    public GameObject lHand;
    public GameObject rHand;
    public Camera cam;
    public float forwardForce = 20.0f;
    public float upForce = 50.0f;

    //RaycastDetect 
    public FootEnterColl foot_l;

    public Rigidbody playerRb;
    private Vector3 lHandVel;
    private Vector3 rHandVel;
    private Vector3 handVeloity;

    private VRIK playerIK;

    private void Start()
    {
        playerIK = this.GetComponentInChildren<VRIK>();
    }

    private void Update()
    {
        if (JumpAction.GetState(SteamVR_Input_Sources.LeftHand) && JumpAction.GetState(SteamVR_Input_Sources.RightHand))
        {
            lHandVel = lHand.GetComponent<SteamVR_Behaviour_Pose>().GetVelocity();
            rHandVel = rHand.GetComponent<SteamVR_Behaviour_Pose>().GetVelocity();

            handVeloity = new Vector3(0, lHandVel.y + rHandVel.y, 0);

            if (handVeloity.magnitude > 2.8f && foot_l.isGround)
            {
                Debug.Log($"HandVel : {handVeloity.magnitude}");

                playerIK.solver.plantFeet = false;
                this.transform.rotation = new Quaternion(0, cam.transform.rotation.y, 0, 0);
                playerRb.AddForce((this.transform.forward * forwardForce + this.transform.up * upForce));

            }
            else playerIK.solver.plantFeet = true;

        }
    }
}
