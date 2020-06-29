using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class FootEnterColl : MonoBehaviour
{
    public bool isGround;
    public Rigidbody playerRB;
    private float distToGround;

    private void Start()
    {
        isGround = true;
        distToGround = this.GetComponent<Collider>().bounds.extents.y;
    }

    private void OnCollisionEnter(Collision coll)
    {
        Debug.Log("Collision Enter");

        if (coll.transform.tag == "GROUND")
            isGround = true;
    }

    private void FixedUpdate()
    {
        //Debug.DrawRay(transform.position, transform.right * (distToGround + 0.15f), Color.blue, 100.0f);

        if (IsGrounded())
        {
            playerRB.useGravity = false;
            isGround = true;
        }
        else
        {
            playerRB.useGravity = true;
            isGround = false;
        }
    }

    private void OnCollisionExit(Collision coll)
    {
        Debug.Log("Collision exit");
        isGround = false;
    }

    bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position, -Vector3.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distToGround + 0.15f, 1 << 19))
        {
            //Debug.Log(hit.transform.name);

            if (hit.transform.tag == "GROUND") return true;
        }

        return false;
    }


}
