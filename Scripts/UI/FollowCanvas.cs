using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FollowCanvas : MonoBehaviour
{
    

    [Header("MyScript")]
    public FollowCanvas followCanvas;

    [Header("Transform")]
    public Transform myTr;
    public Transform UIPosition;


    [Header("DistanceCalculate")]
    public float maxDistance = 0.2f;
    public float teleportDistance = 60f;
    public float dampSpeed = 2f;  // 따라가는 속도
    private float heading;

    private void OnEnable()
    {
        StartCoroutine(CheckTarget());
        StartCoroutine(FollowTarget());
    }

    
    private void ShutDown()
    {
        this.gameObject.SetActive(false);
    }



    IEnumerator CheckTarget()
    {
        while (followCanvas.isActiveAndEnabled)
        {
            heading = (UIPosition.position - myTr.position).sqrMagnitude;
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator FollowTarget()
    {
        while (followCanvas.isActiveAndEnabled)
        {
            // 거리가 멀어지면 실행
            if (heading >= maxDistance)
            {
                myTr.position = Vector3.Lerp(myTr.position
                                           , UIPosition.position
                                           , Time.deltaTime * dampSpeed);
            }
            if (heading > teleportDistance)
            {
                myTr.position = UIPosition.position;
            }
            yield return null;
        }
    }
    private void OnDisable()
    {

    }
}