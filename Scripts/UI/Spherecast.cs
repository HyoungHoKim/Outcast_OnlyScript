using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spherecast : MonoBehaviour
{

    public DroneManager dronState;
    public Transform headTr;
    public float radius = 1f;
    private RaycastHit castHit;
    public float maxDistance = 20f;

    public Transform headShotPref;
    private Transform headShotObj;

    

    private Transform playerUIObj;

    private int droneMask;
    private int enemyMask;
    private bool isDrone;
    private bool isEnemy;

    private Transform prevHit;
    public Transform currHit;


    private Transform hittedTr;
    void Start()
    {
        //헤드샷 UI 생성
        if(headShotPref == null)
            print("헤드샷UI 없음");
        else
            headShotObj = Instantiate(headShotPref, transform.position, transform.rotation);



        headShotObj.gameObject.SetActive(false);
        droneMask = LayerMask.GetMask("DRONE");
        enemyMask = LayerMask.GetMask("ENEMY");
        StartCoroutine(SpherecastHit());
    }
    IEnumerator SpherecastHit()
    {
        while (true)
        {            
            //스피어캐스트가 드론 레이어 오브젝트에 맞았을 때 true
            isDrone = Physics.SphereCast(headTr.position
                                 , radius
                                 , headTr.forward
                                 , out castHit
                                 , maxDistance
                                 , droneMask);


            

            if (isDrone)//드론
            {
                currHit = castHit.transform;
                if (currHit != prevHit)
                {
                    //if(!dronState.enabled)
                    //dronState.enabled = true;
                    //dronState.StartCoroutine("SwitchState");
                }
            }
            else
            {
                //currHit = null;
                if (prevHit != null)
                {
                    //if(dronState.enabled)
                    //dronState.enabled = false;
                    //dronState.StopCoroutine("SwitchState");
                    prevHit = null;
                }
            }


            //스피어캐스트가 에너미 레이어 오브젝트에 맞았을 때 true

            isEnemy = Physics.SphereCast(headTr.position
                                     , radius
                                     , headTr.forward
                                     , out castHit
                                     , maxDistance
                                     , enemyMask);

            if (isEnemy)//적
            {
                currHit = castHit.transform;
                if (currHit != prevHit)
                {
                    headShotObj.position = castHit.transform.position;
                    headShotObj.gameObject.SetActive(true);
                    //GameObject HeadshotHUD = currHit.Find("CanvasHeadshot").gameObject;
                    //HeadshotHUD.SetActive(true);
                    prevHit = currHit;
                }
            }
            else
            {
                if (prevHit != null)
                {
                    headShotObj.gameObject.SetActive(false);
                    //GameObject HeadshotHUD = prevHit.Find("CanvasHeadshot").gameObject;
                    //HeadshotHUD.SetActive(false);
                    prevHit = null;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (isDrone)
        {
            Gizmos.DrawRay(headTr.position, headTr.forward * castHit.distance);
            Gizmos.DrawWireSphere(headTr.position + headTr.forward * castHit.distance, radius);
        }
        else if (isEnemy)
        {
            Gizmos.DrawRay(headTr.position, headTr.forward * castHit.distance);
            Gizmos.DrawWireSphere(headTr.position + headTr.forward * castHit.distance, radius);
        }
        else
        {
            Gizmos.DrawRay(headTr.position, headTr.forward * maxDistance);
        }
    }
}
