using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMove : MonoBehaviour
{
    [Header("MyScript")]
    public DroneMove droneMove;

    [Header("Transform")]
    public Transform myTr;
    private Transform playerTr;

    [Header("DistanceCalculate")]
    private float heading;
    public float maxDistance = 3f;
    public float teleportDistance = 60f;
    public float dampSpeed = 2f;  // 따라가는 속도
    private float posX;
    private float posZ;

    
    private void OnEnable()
    {
        playerTr = Camera.main.GetComponent<Transform>();
        StartCoroutine(CheckTarget());
        StartCoroutine(FollowTarget());
    }
    IEnumerator CheckTarget()
    {
        while (droneMove.isActiveAndEnabled)
        {
            posX = playerTr.position.x - this.myTr.position.x;            
            posZ = playerTr.position.z - this.myTr.position.z;
            heading = (playerTr.position - this.myTr.position).sqrMagnitude;
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator FollowTarget()
    {
        while (droneMove.isActiveAndEnabled)
        {
            if(heading > maxDistance)
            {
                myTr.position = Vector3.Lerp(myTr.position, playerTr.position, 1f * Time.deltaTime);
            }
            // 거리가 멀어지면 실행
            if (posX <= -maxDistance || maxDistance <= posX)
            {
                myTr.position = Vector3.Lerp(myTr.position
                                                 , new Vector3(playerTr.position.x
                                                 , myTr.position.y
                                                 , myTr.position.z)
                                                 , Time.deltaTime * dampSpeed);
            }

            if (posX <= -maxDistance || maxDistance <= posX
            || posZ <= -maxDistance || maxDistance <= posZ)


                myTr.position = Vector3.Lerp(myTr.position
                                                 , new Vector3(myTr.position.x
                                                 , playerTr.position.y
                                                 , myTr.position.z)
                                                 , Time.deltaTime * dampSpeed);


            if (posZ <= -maxDistance || maxDistance <= posZ)
            {
                myTr.position = Vector3.Lerp(myTr.position
                                                 , new Vector3(myTr.position.x
                                                 , myTr.position.y
                                                 , playerTr.position.z)
                                                 , Time.deltaTime * dampSpeed);
            }


            transform.LookAt(playerTr);
            if (heading > teleportDistance)
            {
                myTr.position = new Vector3(playerTr.position.x - 1f, playerTr.position.y, playerTr.position.z - 1f);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            yield return null;
        }
    }
}
