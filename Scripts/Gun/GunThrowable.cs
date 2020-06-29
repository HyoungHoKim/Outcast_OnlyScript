using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GunThrowable : Throwable
{
    protected override void HandHoverUpdate(Hand hand)
    {
        //Debug.Log("Hovering Update");

        GrabTypes startingGrabType = hand.GetGrabStarting();

        if (startingGrabType != GrabTypes.None)
        {
            if (startingGrabType == GrabTypes.Grip)
            {
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags, attachmentOffset);
                //bGunGrip = true;
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

            // Uncomment to detach ourselves late in the frame.
            // This is so that any vehicles the player is attached to
            // have a chance to finish updating themselves.
            // If we detach now, our position could be behind what it
            // will be at the end of the frame, and the object may appear
            // to teleport behind the hand when the player releases it.
            //StartCoroutine( LateDetach( hand ) );
        }
        else if (startingGrabType == GrabTypes.Pinch)
        {
            Debug.Log("Pinch Trigger Down");
            //gameObject.GetComponent<Pistol_02>().Fire();
        }

        if (onHeldUpdate != null)
            onHeldUpdate.Invoke(hand);
    }
}
