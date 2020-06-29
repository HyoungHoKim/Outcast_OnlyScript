using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ShurikenThrowable : Throwable
{
    protected override void OnDetachedFromHand(Hand hand)
    {
        attached = false;

        onDetachFromHand.Invoke();

        hand.HoverUnlock(null);

        rigidbody.interpolation = hadInterpolation;

        Vector3 velocity;
        Vector3 angularVelocity;

        GetReleaseVelocities(hand, out velocity, out angularVelocity);

        rigidbody.isKinematic = false;
        rigidbody.velocity = velocity;
        rigidbody.angularVelocity = angularVelocity;
    }

    private void OnTriggerEnter()
    {
        rigidbody.isKinematic = true;
    }
    
}
