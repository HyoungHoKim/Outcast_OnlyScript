using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneHacking : MonoBehaviour
{
    public enum CurrentState { hackingOn, hackingOff }
    public CurrentState currentState = CurrentState.hackingOff;

    [Header("Script")]
    public DroneManager droneManager;
    public GateAccess gateAccess;
    public HackingPC hackingPC;

    [Header("Transform")]
    public Transform myTr;
    public Transform targetTr;
    public Transform scanBeam;
    public Collider[] gateLocks;

    [Header("Move")]
    private Vector3 heading;
    public float minDistance = 10f;
    public float dampSpeed = 2f;

    [Header("HackingSpeed")]
    public float maxTime = 5.1f;
    private float currTime = 0f;

    [Header("State")]
    public bool isHacking = false;

    private int lockLayer;
    private float scanBeamRotX;

    void Start()
    {
        lockLayer = LayerMask.GetMask("DOORLOCK");
        StartCoroutine(CheckUtileState());
    }

    public IEnumerator StateCheck()
    {
        if (isHacking)
        {
            currentState = CurrentState.hackingOn;
            StartCoroutine(LockCheck());
        }
        yield return null;
    }

    IEnumerator CheckUtileState()
    {
        while (true)
        {
            //idle 상태면 기본이동 켜기
            //기능 실행하면 기본이동 끄기
            //if (currentUtile != CurrentUtile.idle)
                //dronMove.enabled = false;
            //else
                //dronMove.enabled = true;


            //해킹상태가 아니면 타겟 삭제
            if (!isHacking)
                targetTr = null;

            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator LockCheck()
    {
        gateLocks = Physics.OverlapSphere(myTr.position, 5f, lockLayer);

        if (gateLocks.Length >= 1)
        {
            float closestSqr = Mathf.Infinity;  //가장 가까운 거리
            Transform closestTarget = null;     //가장 가까운 타겟
            float targetDist;                   //자신과 타겟과의 거리
            for (int i = 0; i < gateLocks.Length; i++)        //검색
            {
                Vector3 objectPos = gateLocks[i].transform.position;   //찾은 타겟의 포지션
                targetDist = (objectPos - myTr.position).sqrMagnitude;    //자신과 새 타겟과의 현재 거리

                if (targetDist < closestSqr)                  //새 타겟과의 현재 거리가 기존 타겟보다 가까우면
                {
                    closestSqr = targetDist;                  //현재 거리가 가장 가까운 거리가 됨
                    closestTarget = gateLocks[i].transform;    //가장 가까운 타겟은 지금 거리계산한 타겟
                }
            }
            targetTr = closestTarget.transform; //타겟은 가장 가까운 타겟
            if (targetTr.CompareTag("PC"))
                hackingPC = targetTr.GetComponent<HackingPC>();
            else
                gateAccess = targetTr.GetComponent<GateAccess>();

            yield return StartCoroutine(HackingStart());
        }
        else
        {
            print("해킹대상 없음");
            currentState = CurrentState.hackingOff;
            isHacking = false;
            droneManager.currState = DroneManager.CurrentState.idle;
            StartCoroutine(droneManager.SwitchUtile());
            yield return null;
        }        
    }

    IEnumerator HackingStart()
    {
        while (targetTr != null)
        {
            myTr.LookAt(targetTr);
            heading = targetTr.position - myTr.position;
            if (heading.sqrMagnitude > minDistance) //해킹장소가 멀면 이동
            {
                myTr.position = Vector3.Lerp(myTr.position
                                                , targetTr.position
                                                , Time.deltaTime * dampSpeed);
            }
            else //해킹장소가 가까우면
            {
                if (gateAccess != null)
                {
                    gateAccess.enabled = true;
                    gateAccess.isHacking = true;
                }
                if(hackingPC != null)
                {
                    hackingPC.enabled = true;
                    hackingPC.isHacking = true;
                }


                //빔 쏘기
                /*if(!scanBeam.gameObject.activeSelf)
                    scanBeam.gameObject.SetActive(true);

                scanBeamRotX = scanBeam.eulerAngles.x;
                if(scanBeamRotX <= -150f)
                {
                    Vector3.Lerp(myTr.eulerAngles
                               , new Vector3(-30f, 0f, 0f)
                               , Time.deltaTime * dampSpeed);
                }
                if(scanBeamRotX >= -30f)
                {
                    Vector3.Lerp(myTr.eulerAngles
                               , new Vector3(-150f, 0f, 0f)
                               , Time.deltaTime * dampSpeed);
                }*/

                currTime += 1f * Time.deltaTime;
                if (currTime >= maxTime) //일정 시간이 되면
                {
                    targetTr.gameObject.layer = 0;
                    targetTr = null;
                    hackingPC = null;
                    gateAccess = null;
                    isHacking = false;
                    currTime = 0f;
                    currentState = CurrentState.hackingOff;
                    droneManager.currState = DroneManager.CurrentState.idle;
                    StartCoroutine(droneManager.SwitchUtile());
                }
            }
            yield return null;
        }        
    }
}
