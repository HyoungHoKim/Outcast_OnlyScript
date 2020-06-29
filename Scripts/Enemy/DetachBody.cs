using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion;
using RootMotion.Dynamics;

public class DetachBody : MonoBehaviour
{
    public GameObject rightHand;
    public Vector3 startPos;
    public Vector3 endPos;
    public GameObject enemyHead;
    public GameObject enemyHeadRen;
    public PuppetMaster puppetMaster;
    public GameObject EnemyHeadPart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeatchBody()
    {
        
        endPos = rightHand.transform.position;
        float distance = Vector3.Distance(startPos, endPos);
        Debug.Log(distance);
        if (distance > 1)
        {
            EnemyHeadPart.SetActive(true);
            EnemyHeadPart.transform.parent = null;
            enemyHead.SetActive((false));
            enemyHeadRen.SetActive((false));
            puppetMaster.state = PuppetMaster.State.Dead;
        }
       // Debug.Log(Vector3.Distance(startPos.position, endPos.position));
    }

    public void StartPos()

    {
        startPos = rightHand.transform.position;
        Debug.Log("머리잡음");
    }
    
}
