using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBarrier : MonoBehaviour
{
    public enum BarrierState {barrierOn, barrierOff}
    private BarrierState barrierState = BarrierState.barrierOff;

    [Header("Script")]
    public DroneManager droneManager;

    [Header("Transform")]
    public Transform playerTr;
    public Transform myTr;
    public Transform barrierTr;

    [Header("Position")]
    public float maxDistance = 30f;
    public float maxHeight = 2.5f;
    public float dampSpeed = 7f;
    
    [Header("Playtime")]
    public float barrierMaxTime = 10.0f;
    [SerializeField]
    private float currTime;

    [Header("Cooldown")]
    public float barrierDelay = 10.0f;
    [SerializeField]
    private float currDelay;
    public bool barrierPossible = true;
    public bool isBarrier = false;


    private void Start()
    {
        //StartCoroutine(TimeFall());
        //StartCoroutine(MovePosition());
    }
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.X))
        {
            if (barrierTr.gameObject.activeSelf == false)
            {
                //myTr.position = Vector3.Lerp(myTr.position, playerTr.position, Time.deltaTime * dampSpeed); 

                StartCoroutine(BarrierOn());
            }
            else
            {
                   StartCoroutine(BarrierOff());
            }
        }*/
        //loat heading = (playerTr.up - myTr.up).sqrMagnitude;
        //print(heading);
    }

    public IEnumerator StateCheck()
    {
        if (isBarrier)
        {
            barrierState = BarrierState.barrierOn;
            yield return StartCoroutine(BarrierOn());            
        }
        else
        {
            barrierState = BarrierState.barrierOff;
            yield return StartCoroutine(BarrierOff());
        }
        
    }


    IEnumerator BarrierOn()
    {
        barrierTr.localScale = new Vector3(4.5f, 4.5f, 4.5f);
        barrierTr.gameObject.SetActive(true);
        barrierPossible = false;
        StartCoroutine(MovePosition());
        yield return StartCoroutine(TimeFall());
    }

    IEnumerator TimeFall()
    {
        while (!barrierPossible)
        {
            //시전 시간
            if (isBarrier)
            {
                currTime += 1f * Time.deltaTime;

                if (currTime >= barrierMaxTime)
                {
                    StartCoroutine(BarrierOff());
                    currTime = 0f;
                }
            }

            //배리어 쿨타임
            if (!isBarrier && !barrierPossible)
            {
                currDelay += 1f * Time.deltaTime;
                if (currDelay >= barrierDelay) //일정시간 지나면 배리어 사용 가능
                {
                    barrierPossible = true;
                    currDelay = 0f;
                }
            }
            yield return null;
        }
    }

    IEnumerator BarrierOff()
    {
        while (barrierTr.gameObject.activeSelf)
        {
            barrierTr.localScale = Vector3.Lerp(barrierTr.localScale
                                             , barrierTr.localScale * 0f
                                             , Time.deltaTime * dampSpeed);
            if (barrierTr.localScale.y <= 0.1f)
            {
                barrierTr.gameObject.SetActive(false);
                isBarrier = false;
                droneManager.currState = DroneManager.CurrentState.idle;
                StartCoroutine(droneManager.SwitchUtile());
                currTime = 0f;      //시간 초기화
            }
            yield return null;
        }
    }


    IEnumerator MovePosition()
    {
        while (isBarrier)
        {
            myTr.position = Vector3.Lerp(myTr.position
                                          , new Vector3
                                           (playerTr.position.x
                                          , playerTr.position.y + maxHeight
                                          , playerTr.position.z)
                                          , Time.deltaTime * dampSpeed);

            myTr.LookAt(playerTr);
            yield return null;
        }
    }

    

    
}
