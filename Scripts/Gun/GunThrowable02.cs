using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GunThrowable02 : Throwable
{
    //Magazine
    public Transform InitialPos;
    private string MagTag;

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
        }
        else if (startingGrabType == GrabTypes.Pinch)
        {
            Debug.Log("Pinch Trigger Down");

            if (MagTag == "MAGAZINE_REFLECT")
            {
                gameObject.GetComponent<Pistol_02>().beaming = false;
                gameObject.GetComponent<Pistol_02>().FireBeam();

                Invoke("reStartReBeam", 0.1f);
            }
            //else if (MagTag == "MAGAZINE") gameObject.GetComponent<Pistol_02>().Fire();
        }

        if (onHeldUpdate != null)
            onHeldUpdate.Invoke(hand);
    }

    void reStartReBeam()
    {
        gameObject.GetComponent<Pistol_02>().beaming = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MAGAZINE"))
        {
            other.GetComponentInParent<Valve.VR.InteractionSystem.Hand>().DetachObject(other.gameObject, false);
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<Rigidbody>().isKinematic = true;


            other.transform.position = InitialPos.position;
            other.transform.rotation = InitialPos.rotation;
            other.transform.SetParent(this.transform);

            GetComponent<Pistol>().Reload();

            MagTag = "MAGAZINE";
        }
        else if (other.CompareTag("MAGAZINE_REFLECT"))
        {
            other.GetComponentInParent<Valve.VR.InteractionSystem.Hand>().DetachObject(other.gameObject, false);
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<Rigidbody>().isKinematic = true;


            other.transform.position = InitialPos.position;
            other.transform.rotation = InitialPos.rotation;
            other.transform.SetParent(this.transform);

            GetComponent<Pistol>().Reload();
            gameObject.GetComponent<Pistol_02>().Beam();

            MagTag = "MAGAZINE_REFLECT";
        }
    }
}
