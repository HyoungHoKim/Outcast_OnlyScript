using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class DroneManager : MonoBehaviour
{
    public static DroneManager instance;

    public enum CurrentItem
    { 
        none
      , pistol
      , magazine
      , refMagazine
      , knife
      , bomb
    }
    public CurrentItem currItem = CurrentItem.none;



    public enum CurrentState
    {
        idle
      , hacking
      , barrier
      , sonar
    }

    public CurrentState currState = CurrentState.idle;

    [Header("VR")]
    private SteamVR_Action_Boolean ClimbAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    private SteamVR_Input_Sources inputSource;

    [Header("DroneScripts")]
    public DroneItem droneItem;
    public DroneHacking droneHacking;
    public DroneBarrier droneBarrier;
    public DroneSonar droneSonar;
    public DroneMove droneMove;



    [Header("ManagerUtile")]
    private float currTime;
    public float noneMaxTime = 3.0f;
    public AudioClip[] audioClip = new AudioClip[9];
    private int clipNumber;
    private AudioSource audioSource;

    

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }


    private void Update()
    {
        InputInterface();
        ToStateNone();
    }

    public void InputInterface()
    {
        /*switch (Input.inputString)  //키입력
        {
            //드론 아이템
            case "1":
                currItem = CurrentItem.pistol;
                StartCoroutine(SwitchItem());
                break;

            case "2":
                currItem = CurrentItem.magazine;
                StartCoroutine(SwitchItem());
                break;

            case "3":
                currItem = CurrentItem.refMagazine;
                StartCoroutine(SwitchItem());
                break;

            case "4":
                currItem = CurrentItem.knife;
                StartCoroutine(SwitchItem());
                break;


                //드론 유틸
                //기능을 사용하고 있을 때 다른기능 사용 못함
                
            
            //해킹
            case "z":
                if(!droneBarrier.isBarrier //배리어 사용 false일 때
                && !droneSonar.isSonar)    //소나 사용 false일 때
                {
                    if(currState != CurrentState.hacking)
                    {
                        currState = CurrentState.hacking;
                        StartCoroutine(SwitchUtile());
                    }
                }                
                break;

            //배리어
            case "x":
                if (!droneHacking.isHacking
                 && !droneSonar.isSonar)
                {
                    if (droneBarrier.barrierPossible)//배리어 사용 가능할 때
                        currState = CurrentState.barrier;
                    else
                    {
                        droneBarrier.isBarrier = false;
                        droneBarrier.StartCoroutine(droneBarrier.StateCheck());
                    }
                    StartCoroutine(SwitchUtile());
                }                    
                break;
                
            //소나
            case "c":
                if (!droneBarrier.isBarrier
                 && !droneHacking.isHacking)
                {
                    if (currState != CurrentState.sonar)
                        currState = CurrentState.sonar;
                    else
                        currState = CurrentState.idle;

                    StartCoroutine(SwitchUtile());
                }
                break;
        }*/
    }

    public void DronePistol()
    {
        currItem = CurrentItem.pistol;
        StartCoroutine(SwitchItem());
    }
    public void DroneMagazine()
    {
        currItem = CurrentItem.magazine;
        StartCoroutine(SwitchItem());
    }
    public void DroneRefMagazine()
    {
        currItem = CurrentItem.refMagazine;
        StartCoroutine(SwitchItem());
    }
    public void DroneKnife()
    {
        currItem = CurrentItem.knife;
        StartCoroutine(SwitchItem());
    }

    public void DroneBomb()
    {
        currItem = CurrentItem.bomb;
        StartCoroutine(SwitchItem());
    }
    public void DroneHacking()
    {
        if (!droneBarrier.isBarrier //배리어 사용 false일 때
                && !droneSonar.isSonar)    //소나 사용 false일 때
        {
            if (currState != CurrentState.hacking)
            {
                currState = CurrentState.hacking;
                StartCoroutine(SwitchUtile());
            }
        }
    }
    public void DroneStealth()
    {
        if (!droneHacking.isHacking
                 && !droneSonar.isSonar)
        {
            if (droneBarrier.barrierPossible)//배리어 사용 가능할 때
                currState = CurrentState.barrier;
            else
            {
                droneBarrier.isBarrier = false;
                droneBarrier.StartCoroutine(droneBarrier.StateCheck());
            }
            StartCoroutine(SwitchUtile());
        }
    }
    public void DroneSonar()
    {
        if (!droneBarrier.isBarrier
                 && !droneHacking.isHacking)
        {
            if (currState != CurrentState.sonar)
                currState = CurrentState.sonar;
            else
                currState = CurrentState.idle;

            StartCoroutine(SwitchUtile());
        }
    }

    IEnumerator SwitchItem()
    {
        switch (currItem)
        {
            case CurrentItem.none:
                clipNumber = 0;
                AudioPlay();
                droneItem.ObjectCheck();
                break;



            case CurrentItem.pistol:
                clipNumber = 1;
                AudioPlay();
                droneItem.CreatePistol();
                break;



            case CurrentItem.magazine:
                clipNumber = 2;
                AudioPlay();
                droneItem.CreateMagazine();
                break;



            case CurrentItem.refMagazine:
                clipNumber = 3;
                AudioPlay();
                droneItem.CreateReflectionMG();
                break;



            case CurrentItem.knife:
                clipNumber = 4;
                AudioPlay();
                droneItem.CreateKnife();
                break;


            case CurrentItem.bomb:
                clipNumber = 5;
                AudioPlay();
                droneItem.CreateBomb();
                break;
        }
        yield return null;
    }

    public IEnumerator SwitchUtile()
    {
        switch (currState)
        {
            case CurrentState.idle:
                droneSonar.isSonar = false;
                droneSonar.StartCoroutine(droneSonar.StateCheck());
                
                droneMove.enabled = true;
                break;



            case CurrentState.hacking:
                droneMove.enabled = false;
                droneHacking.isHacking = true;
                droneHacking.StartCoroutine(droneHacking.StateCheck());
                break;



            case CurrentState.barrier:
                droneMove.enabled = false;
                droneBarrier.isBarrier = true;
                droneBarrier.StartCoroutine(droneBarrier.StateCheck());
                break;



            case CurrentState.sonar:
                droneMove.enabled = false;
                droneSonar.isSonar = true;
                droneSonar.StartCoroutine(droneSonar.StateCheck());
                break;
        }
        yield return null;
    }

    private void AudioPlay()
    {
        if (audioClip[clipNumber] != null)
            audioSource.PlayOneShot(audioClip[clipNumber]);    
    }

    private void ToStateNone()
    {
        if(currItem != CurrentItem.none)
        {
            currTime += 1f * Time.deltaTime;
            if (currTime >= noneMaxTime) //일정 시간이 되면
            {
                currItem = CurrentItem.none; //none으로 바뀜
                StartCoroutine(SwitchItem());
                currTime = 0f;      //시간 초기화
            }


            if (ClimbAction.GetStateUp(inputSource))//키 입력을 받으면
            {
                currTime = 0f;   //none로 변경하는 시간 초기화
            }
        }        
    }
}
