using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MagThrowable : Throwable
{

    protected override void HandHoverUpdate(Hand hand)
    {
        //Debug.Log("Hovering Update");

        GrabTypes startingGrabType = hand.GetGrabStarting();

        if (startingGrabType != GrabTypes.None)
        {
            if (startingGrabType == GrabTypes.Grip)
            {
                if(gameObject.transform.parent != null)
                {
                    if (gameObject.transform.parent.CompareTag("PISTOL"))
                    {
                        gameObject.transform.parent.GetComponent<Pistol_02>().CurrentAmmo = 0;

                        if (gameObject.CompareTag("MAGAZINE_REFLECT"))
                            gameObject.transform.parent.GetComponent<Pistol_02>().beaming = false;
                            gameObject.transform.parent.GetComponent<Pistol_02>().StopBeam();
                    }
                }

                hand.AttachObject(gameObject, startingGrabType, attachmentFlags, attachmentOffset);
            }

            hand.HideGrabHint();
        }
    }

    protected override void HandAttachedUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();

        if (startingGrabType == GrabTypes.Grip)
        {

            hand.DetachObject(gameObject, restoreOriginalParent);
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
        }

        if (onHeldUpdate != null)
            onHeldUpdate.Invoke(hand);
    }
}
