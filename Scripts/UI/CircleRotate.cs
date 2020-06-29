using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleRotate : MonoBehaviour
{
    public Transform parentTr;
    public RectTransform tr;

    [Header("RandomAngle")]
    public float minAngle = 10f;
    public float maxAngle = 180f;


    private float prevAngleZ;
    private float currAngleZ;

    [Header("RandomSpeed")]
    public float minSpeed = 0.5f;
    public float maxSpeed = 1f;

    [Header("RandomTime")]
    public float minTime = 0.5f;
    public float maxTime = 1.5f;

    private float rotateSpeed;
    private float currTime;
    private float changeTime;


    public bool isTouched = false;
    private void OnEnable()
    {
        //tr = this.GetComponent<RectTransform>();
        //tr.rotation = Quaternion.Euler(Vector3.zero);
        
        StartCoroutine(RandomFloat());
    }
    IEnumerator RandomFloat()
    {
        
        changeTime = Random.Range(minTime, maxTime);
        rotateSpeed = Random.Range(minSpeed, maxSpeed);
        //int plusMinus = Random.Range(0, 2);
        /*if (currAngleZ >= 0f)
        {
            currAngleZ = Random.Range(minAngle, maxAngle);
            currAngleZ = -currAngleZ;
        }            
        else*/
            currAngleZ = Random.Range(minAngle, maxAngle);
        float a = Mathf.Abs(prevAngleZ - currAngleZ);
        if (a <= 50f)
        {
            currAngleZ += 180f;
        }
        prevAngleZ = currAngleZ;

        yield return StartCoroutine(RotationPlay());
    }

    IEnumerator RotationPlay()
    {
        while (true)
        {
            //rot = Vector3.Lerp(tr.rotation.eulerAngles, tr.forward, rotateSpeed * Time.deltaTime);
            //tr.Rotate(0, 0, currAngleZ * (rotateSpeed * Time.deltaTime));
            //int a = Random.Range(1, 3);
            tr.localRotation = Quaternion.Lerp(tr.localRotation, Quaternion.Euler(tr.localRotation.x, tr.localRotation.y, currAngleZ), rotateSpeed * Time.deltaTime);

            currTime += Time.deltaTime;
            if (currTime >= changeTime)
            {
                currTime = 0f;
                yield return StartCoroutine(RandomFloat());
                break;
            }
            yield return null;
        }
    }
}
