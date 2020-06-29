using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.EventSystems;

public class Finger : MonoBehaviour
{
    public enum UIState{ off, item, drone, menu};
    public UIState uiState = UIState.off;

    [Header("Script")]
    public ButtonSelect buttonSelect; //나중에 숨기기

    [Header("Transform")]
    public Transform uiPosition;
    public Transform fingerTr;

    [Header("UI_Transform")]
    public Transform itemPanel;
    public Transform dronePanel;
    public Transform menuPanel;
    public Transform prevPos;
    public Transform currPos;
    public Transform nextPos;
    public float moveSpeed = 1f;
    public float currTime;
    public float maxTime = 1.5f;
    public bool isSlide = false;

    [Header("Animation")]
    public Animator anim;
    private bool isItem = false;
    private bool isDrone = false;
    private bool isMenu = false;

    [Header("VR")]
    private SteamVR_Action_Boolean ClimbAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    public SteamVR_Action_Boolean GripAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    private SteamVR_Input_Sources inputSource;
    private SteamVR_Behaviour_Pose hand;
    private float velocity;

    [Header("RayCast")]
    private int buttonMask;
    private RaycastHit hitButton;
    public float dist = 10.0f;

    [Header("Canvas")]
    public Transform uiCanvas;

    [Header("Button")]
    public GameObject currButton; //나중에 숨기기
    public GameObject prevButton; //나중에 숨기기
    public bool isButton;

    private void Start()
    {
        buttonMask = LayerMask.GetMask("BUTTON");
        hand = GameObject.Find("Controller (left)").GetComponent<SteamVR_Behaviour_Pose>();
        StartCoroutine(UIButtonCheck());
    }
    IEnumerator UIButtonCheck()
    {
        while (true)
        {
            if (ClimbAction.GetState(inputSource))
            {                
                velocity = hand.transform.InverseTransformDirection(hand.GetVelocity()).x;
                //print(velocity);
                //velocity = hand.GetVelocity().x;
                if (velocity >= 1f)
                {
                    if (!uiCanvas.gameObject.activeSelf)
                    {
                        uiCanvas.position = uiPosition.position;
                        uiCanvas.gameObject.SetActive(true);
                        uiState = UIState.item;
                        StartCoroutine(RaycastEvent());
                        yield return null;
                    }
                    else
                    {
                        switch (uiState)
                        {
                            case UIState.item:
                                if(isSlide)
                                    uiState = UIState.drone;
                                break;
                            case UIState.drone:
                                if (isSlide)
                                    uiState = UIState.menu;
                                break;
                            case UIState.menu:
                                if (isSlide)
                                    uiState = UIState.item;
                                break;
                        }
                    }
                    yield return StartCoroutine(UIStateCheck());
                }
                if (velocity <= -1f)
                {

                    uiState = UIState.off;
                    
                    yield return StartCoroutine(UIStateCheck());
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator UIStateCheck()
    {
        switch (uiState)
        {
            case UIState.off:
                if (uiCanvas.gameObject.activeSelf)
                {
                    //uiCanvas.position = fingerTr.position;
                    uiCanvas.gameObject.SetActive(false);
                    isSlide = false;
                }
                break;
            case UIState.item:
                uiCanvas.position = uiPosition.position;
                
                //활성화
                itemPanel.gameObject.SetActive(true);
                dronePanel.gameObject.SetActive(false);
                menuPanel.gameObject.SetActive(false);
                
                //애니메이션 활성화
                anim.SetBool("ITEM", true);
                anim.SetBool("DRONE", false);
                anim.SetBool("MENU", false);
                isSlide = false;
                break;
            case UIState.drone:

                //활성화
                uiCanvas.position = uiPosition.position;
                itemPanel.gameObject.SetActive(false);
                dronePanel.gameObject.SetActive(true);
                menuPanel.gameObject.SetActive(false);

                //애니메이션 활성화
                anim.SetBool("ITEM", false);
                anim.SetBool("DRONE", true);
                anim.SetBool("MENU", false);
                isSlide = false;
                break;
            case UIState.menu:

                //활성화
                uiCanvas.position = uiPosition.position;
                itemPanel.gameObject.SetActive(false);
                dronePanel.gameObject.SetActive(false);
                menuPanel.gameObject.SetActive(true);

                //애니메이션 활성화
                anim.SetBool("ITEM", false);
                anim.SetBool("DRONE", false);
                anim.SetBool("MENU", true);
                isSlide = false;
                break;
        }
        yield return null;
    }
    IEnumerator RaycastEvent()
    {        
        while (uiCanvas.gameObject.activeSelf)
        {
            if (!isSlide)
            {
                currTime += 1f * Time.deltaTime;
                if (currTime >= maxTime)
                {
                    isSlide = true;
                    currTime = 0f;
                }
            }
            isButton = Physics.Raycast(fingerTr.position
                                 , fingerTr.forward
                                 , out hitButton
                                 , dist
                                 , buttonMask);

            if (isButton) //레이에 맞았을 때
            {
                currButton = hitButton.collider.gameObject; //현재 버튼

                buttonSelect = currButton.GetComponent<ButtonSelect>(); //현재 버튼 스크립트 받아옮
                buttonSelect.PanelOn();

                if (currButton != prevButton) //현재 버튼과 이전 버튼이 다를 때
                {
                    if (prevButton != null)
                    {
                        buttonSelect = prevButton.GetComponent<ButtonSelect>(); //이전 버튼 스크립트를 받아옮
                        buttonSelect.PanelOff();

                    }
                    prevButton = currButton; //현재 버튼을 이전버튼에 저장
                }


                PointerEventData data = new PointerEventData(EventSystem.current); //이벤트 시스템

                if (ClimbAction.GetStateDown(inputSource)) //바이브 트리거 클릭
                    ExecuteEvents.Execute(currButton, data, ExecuteEvents.pointerClickHandler); //클릭 이벤트
            }
            else //레이에 맞지 않았을 때
            {
                if (buttonSelect != null)
                {
                    buttonSelect.PanelOff();
                    buttonSelect = null; //버튼 스크립트 삭제
                }

                if (prevButton != null)
                    prevButton = null; //이전 버튼 삭제
                if (currButton != null)
                    currButton = null; //현재 버튼 삭제
            }
            //Debug.DrawRay(fingerTr.position, fingerTr.forward * hit.distance, Color.green);

            /*if (Physics.Raycast(ray, out hit, dist, 1<<16))
            {

                ButtonPlay();
                //print(hit.collider.gameObject.layer);
            }
            else
                ReleaseButton();*/
            //Debug.DrawRay(fingerTr.position, -fingerTr.up, Color.green);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (isButton)
            Gizmos.DrawRay(fingerTr.position, fingerTr.forward * hitButton.distance);
        else
            Gizmos.DrawRay(fingerTr.position, fingerTr.forward * dist);
    }

    /*void ButtonPlay()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        print(data);
        if (hit.collider.gameObject.layer == 16)
        {
            currButton = hit.collider.gameObject;
            uIHighlight = currButton.GetComponent<UIHighlight>();
            uIHighlight.ButtonSelectedOn();

            if (ClimbAction.GetStateDown(inputSource))
            {
                ExecuteEvents.Execute(currButton, data, ExecuteEvents.pointerClickHandler);
            }
            if (currButton != prevButton)
            {
                ExecuteEvents.Execute(currButton, data, ExecuteEvents.pointerEnterHandler);
                ExecuteEvents.Execute(prevButton, data, ExecuteEvents.pointerExitHandler);
                prevButton = currButton;
            }
        }
        else
            ReleaseButton();
    }*/
    /*void ReleaseButton()
    {     
        PointerEventData data = new PointerEventData(EventSystem.current);
        currButton = null;
        if (prevButton != null)
        {
            ExecuteEvents.Execute(prevButton, data, ExecuteEvents.pointerExitHandler);
            
            prevButton = null;
        }
    }*/
    
}
