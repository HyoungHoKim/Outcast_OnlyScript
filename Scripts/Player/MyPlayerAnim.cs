using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MyPlayerAnim : MonoBehaviour
{

    public Animator playerAnimator;

    public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    public SteamVR_Action_Boolean grabGripAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");

    public PickUp lHand;
    public PickUp rHand;

    void FixedUpdate()
    {
       handAnim();
    }

    void handAnim()
    {
        if (grabGripAction.GetState(SteamVR_Input_Sources.LeftHand) && !lHand.isGrabbing)
        {
            playerAnimator.SetBool("bLeftGrip", true);
        }

        if (grabPinchAction.GetState(SteamVR_Input_Sources.LeftHand) && !lHand.isGrabbing)
        {
            playerAnimator.SetBool("bLeftTrigger", true);
        }

        if (grabGripAction.GetState(SteamVR_Input_Sources.RightHand) && !rHand.isGrabbing)
        {
            playerAnimator.SetBool("bRightGrip", true);
        }

        if (grabPinchAction.GetState(SteamVR_Input_Sources.RightHand) && !rHand.isGrabbing)
        {
            playerAnimator.SetBool("bRightTrigger", true);
        }

        //-----------------------------------------------------------------

        if (grabGripAction.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            playerAnimator.SetBool("bLeftGrip", false);
        }

        if (grabPinchAction.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            playerAnimator.SetBool("bLeftTrigger", false);
        }

        if (grabGripAction.GetStateUp(SteamVR_Input_Sources.RightHand))
        {
            playerAnimator.SetBool("bRightGrip", false);
        }

        if (grabPinchAction.GetStateUp(SteamVR_Input_Sources.RightHand))
        {
            playerAnimator.SetBool("bRightTrigger", false);
        }
    }
}