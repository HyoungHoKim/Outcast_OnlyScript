using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorColl : MonoBehaviour
{
    Rigidbody doorRig;
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("Respawn"))
        {
            doorRig = coll.gameObject.GetComponent<Rigidbody>();
            doorRig.velocity = Vector3.zero;
        }
    }
}
