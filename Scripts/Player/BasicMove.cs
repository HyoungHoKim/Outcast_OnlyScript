using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BasicMove : MonoBehaviour
{
    public SteamVR_Action_Vector2 MoveAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("BasicMove");
    public SteamVR_Action_Boolean TeleportAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");
    public Transform cam;
    public FootEnterColl lCheckGround;
    public FootEnterColl rCheckGround;

    public float speed = 5;

    private Vector2 axis;

    private void Awake()
    {
        axis = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //bool checkGround = lCheckGround.isGround || rCheckGround.isGround;

        //Debug.Log("walk");

        if (!TeleportAction.GetState(SteamVR_Input_Sources.Any)) // if you want to move Ground Layer, add checkGround 
        { 
            axis = MoveAction.axis;

            Vector3 dirY = new Quaternion(0, cam.transform.localRotation.y, 0, cam.transform.localRotation.w) * Vector3.forward;
            Vector3 dirX = new Quaternion(0, cam.transform.localRotation.y, 0, cam.transform.localRotation.w) * Vector3.right;

            this.transform.position += (dirX * axis.x + dirY * axis.y) * Time.deltaTime * speed;
        }

    }

}
