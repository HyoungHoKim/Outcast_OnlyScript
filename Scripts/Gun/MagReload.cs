using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MagReload : MonoBehaviour
{
    public Transform InitialPos;

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

        }
    }
}


