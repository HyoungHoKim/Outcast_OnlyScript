using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using RootMotion.Demos;
using Valve.VR;
using UnityEngine.XR;

public class PistolPickUp : MonoBehaviour
{
    public SteamVR_Action_Boolean GripAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    public SteamVR_Action_Boolean PinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");

    //magazine
    private string MagTag = "MAGAZINE";
    public Transform InitialPos;

    private void Update()
    { 

        if (GripAction.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            if (this.transform.parent != null && this.transform.parent.name == "hand_r")
            {
                Debug.Log("RightDrop!!");

            }
        }
        else if (GripAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            if (this.transform.parent != null && this.transform.parent.name == "hand_l")
            {
                Debug.Log("LeftDrop!!");

            }
        }

        FireAction();
    }

    void FireAction()
    {

        if (PinchAction.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            if (this.transform.parent != null && this.transform.parent.name == "hand_r")
            {
                Debug.Log("RightFire!!");

                if (MagTag == "MAGAZINE_REFLECT")
                {
                    gameObject.GetComponent<Pistol_02>().beaming = false;
                    gameObject.GetComponent<Pistol_02>().FireBeam();

                    Invoke("reStartReBeam", 0.1f);
                }
                else if (MagTag == "MAGAZINE")
                {
                    gameObject.GetComponent<Pistol_02>().Fire();
                }
            }
        }
        else if (PinchAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            if (this.transform.parent != null && this.transform.parent.name == "hand_l")
            {
                Debug.Log("LeftFire!!");

                if (MagTag == "MAGAZINE_REFLECT")
                {
                    gameObject.GetComponent<Pistol_02>().beaming = false;
                    gameObject.GetComponent<Pistol_02>().FireBeam();

                    Invoke("reStartReBeam", 0.1f);
                }
                else if (MagTag == "MAGAZINE")
                {
                    gameObject.GetComponent<Pistol_02>().Fire();
                }
            }
        }
    }

    void reStartReBeam()
    {
        gameObject.GetComponent<Pistol_02>().beaming = true;
    }

   
}
