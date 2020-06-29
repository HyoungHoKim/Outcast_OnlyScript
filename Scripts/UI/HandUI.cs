using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandUI : MonoBehaviour
{
    [Header("Script")]
    private ButtonEvent buttonEvent;

    [Header("VR")]
    private SteamVR_Action_Boolean ClimbAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    private SteamVR_Input_Sources inputSource;

    [Header("UI LIST")]
    public List<Transform> uiTrs;

    [Header("Transform")]
    public Transform fingerTr;
    public Transform targetTr;
    private Transform tr;

    [Header("Bool")]
    public bool isUiTouched = false;


    private void Start()
    {
        tr = this.GetComponent<Transform>();
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("BUTTON"))
        {
            isUiTouched = true;
            tr.position = Vector3.Lerp(tr.position, other.transform.position, Time.deltaTime);
            uiTrs.Add(other.transform);


            if (uiTrs.Count >= 2)
                StartCoroutine(UIReaction());
            else
            {
                targetTr = other.transform;
                ButtonScriptOn();
            }
        }
    }*/

    /*private void OnTriggerExit(Collider other)
    {
        isUiTouched = false;
        uiTrs.Clear();
    }*/

    IEnumerator UIReaction()
    {
        float closestSqr = Mathf.Infinity;  //가장 가까운 거리
        Transform closestTarget = null;     //가장 가까운 타겟
        float targetDist;                   //자신과 타겟과의 거리
        for (int i = 0; i < uiTrs.Count; i++)        //검색
        {
            Vector3 objectPos = uiTrs[i].transform.position;   //찾은 타겟의 포지션
            targetDist = (objectPos - fingerTr.position).sqrMagnitude;    //자신과 새 타겟과의 현재 거리

            if (targetDist < closestSqr)                  //새 타겟과의 현재 거리가 기존 타겟보다 가까우면
            {
                closestSqr = targetDist;                  //현재 거리가 가장 가까운 거리가 됨
                closestTarget = uiTrs[i].transform;    //가장 가까운 타겟은 지금 거리계산한 타겟
            }
        }
        targetTr = closestTarget.transform; //타겟은 가장 가까운 타겟
        ButtonScriptOn();
        yield return StartCoroutine(VRInput());
    }

    private void ButtonScriptOn()
    {
        buttonEvent = targetTr.GetComponent<ButtonEvent>();
        buttonEvent.isTrigger = true;
    }


    IEnumerator VRInput()
    {
        while (isUiTouched)
        {
            if (ClimbAction.GetStateUp(inputSource))
            {
                buttonEvent.ButtonPlay();
            }
            yield return null;
        }
    }
}
