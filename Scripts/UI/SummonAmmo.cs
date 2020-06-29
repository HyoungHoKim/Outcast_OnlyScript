using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SummonAmmo : MonoBehaviour
{
    public SteamVR_Action_Boolean GripAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    public Transform leftController;
    public Transform rightController;

    private Transform Controller;

    private Transform obj;
    //public float radius = 1f;
    //public float maxDistance = 20f;
    private DroneManager droneManager;
    private DroneItem droneItem;
    //private RaycastHit castHit;
    //private int itemMask;

    //private bool isItem;

    //private Transform prevHit;
    //private Transform currItem;
    private Rigidbody itemRig;

    public Transform lKnifePos;
    public Transform lMagAttachPos;
    public Transform lPistolAttachPos;

    public Transform rKnifePos;
    public Transform rMagAttachPos;
    public Transform rPistolAttachPos;

    private Transform knifePos;
    private Transform magAttachPos;
    private Transform pistolAttachPos;

    Vector3 destination;
    private float dist;
    public float attachSpeed = 4f;
    public float attachDist = 0.5f;
    void Start()
    {
        droneManager = this.GetComponent<DroneManager>();
        droneItem = this.GetComponent<DroneItem>();
        //itemMask = LayerMask.GetMask("ITEM");
        //StartCoroutine(SpherecastHit());
        //StartCoroutine(SummonItem());
    }
    private void Update()
    {
        if (GripAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            knifePos = lKnifePos;
            magAttachPos = lMagAttachPos;
            pistolAttachPos = lPistolAttachPos;
            Controller = leftController;

            SummonAmmoItem();
        }
        else if (GripAction.GetStateDown(SteamVR_Input_Sources.RightHand))
        { 
            knifePos = rKnifePos;
            magAttachPos = rMagAttachPos;
            pistolAttachPos = rPistolAttachPos;
            Controller = rightController;

            SummonAmmoItem();
        }
            
    }
    /*IEnumerator SpherecastHit()// 사용안함
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            
            
            //스피어캐스트가 아이템 레이어 오브젝트에 맞았을 때 true
            isItem = Physics.SphereCast(transform.position
                                 , radius
                                 , transform.forward
                                 , out castHit
                                 , maxDistance
                                 , itemMask);
            
            if (isItem)
            {
                currItem = castHit.collider.gameObject.transform;
            }
        }
    }*/
    public void SummonAmmoItem()
    {
        if(droneManager.currItem == DroneManager.CurrentItem.pistol)
        {
            obj = droneItem.ammo[0];
            itemRig = obj.GetComponent<Rigidbody>();
            destination = pistolAttachPos.position - obj.position;
            itemRig.AddForce(destination * attachSpeed, ForceMode.Impulse);
            StartCoroutine(SummonPistol());
        }
        if (droneManager.currItem == DroneManager.CurrentItem.magazine)
        {
            obj = droneItem.ammo[0];
            itemRig = obj.GetComponent<Rigidbody>();
            destination = magAttachPos.position - obj.position;
            itemRig.AddForce(destination * attachSpeed, ForceMode.Impulse);
            StartCoroutine(SummonMagazine());
        }
        if (droneManager.currItem == DroneManager.CurrentItem.refMagazine)
        {
            obj = droneItem.ammo[0];
            itemRig = obj.GetComponent<Rigidbody>();
            destination = magAttachPos.position - obj.position;
            itemRig.AddForce(destination * attachSpeed, ForceMode.Impulse);
            StartCoroutine(SummonRefMagazine());
        }
        if (droneManager.currItem == DroneManager.CurrentItem.knife)
        {
            obj = droneItem.ammo[0];
            itemRig = obj.GetComponent<Rigidbody>();
            destination = knifePos.position - obj.position;
            itemRig.AddForce(destination * attachSpeed, ForceMode.Impulse);
            StartCoroutine(SummonKnife());
        }
        if (droneManager.currItem == DroneManager.CurrentItem.bomb)
        {
            obj = droneItem.ammo[0];
            itemRig = obj.GetComponent<Rigidbody>();
            destination = knifePos.position - obj.position;
            itemRig.AddForce(destination * attachSpeed, ForceMode.Impulse);
            StartCoroutine(SummonBomb());
        }

    }

    IEnumerator SummonPistol()
    {
        while(pistolAttachPos.childCount == 0)
        {
            destination = pistolAttachPos.position - droneItem.ammo[0].position;
            dist = destination.magnitude; //거리체크
            if (dist <= 0.2f)
            {
                //itemRig.useGravity = false;
                itemRig.velocity = Vector3.zero;             //가속도 초기화

                Controller.GetComponent<PickUp>().DroneAttach(obj.gameObject);
            }
            yield return null;
        }
    }

    IEnumerator SummonMagazine()
    {
        while (magAttachPos.childCount == 0)
        {
            destination = magAttachPos.position - droneItem.ammo[0].position;
            dist = destination.magnitude; //거리체크
            if (dist <= 0.1f)
            {
                //itemRig.useGravity = false;
                itemRig.velocity = Vector3.zero;             //가속도 초기화

                Controller.GetComponent<PickUp>().DroneAttach(obj.gameObject);
            }
            yield return null;
        }
    }

    IEnumerator SummonRefMagazine()
    {
        while (magAttachPos.childCount == 0)
        {
            destination = magAttachPos.position - droneItem.ammo[0].position;
            dist = destination.magnitude; //거리체크
            if (dist <= 0.1f)
            {
                //itemRig.useGravity = false;
                itemRig.velocity = Vector3.zero;             //가속도 초기화
                //print(itemRig.velocity);

                Controller.GetComponent<PickUp>().DroneAttach(obj.gameObject);
            }
            yield return null;
        }
    }

    IEnumerator SummonKnife()
    {
        while (knifePos.childCount == 0)
        {
            destination = knifePos.position - droneItem.ammo[0].position;
            dist = destination.magnitude; //거리체크
            if (dist <= attachDist)
            {
                //itemRig.useGravity = false;
                itemRig.velocity = Vector3.zero;             //가속도 초기화

                Controller.GetComponent<PickUp>().DroneAttach(obj.gameObject);
            }
            yield return null;
        }
    }

    IEnumerator SummonBomb()
    {
        while (knifePos.childCount == 0)
        {
            destination = knifePos.position - droneItem.ammo[0].position;
            dist = destination.magnitude; //거리체크
            
            if (dist <= attachDist)
            {
                //itemRig.useGravity = false;
                itemRig.velocity = Vector3.zero;             //가속도 초기화

                Controller.GetComponent<PickUp>().DroneAttach(obj.gameObject);
            }
            yield return null;
        }
    }
    /*IEnumerator SummonItem()
    {
        while (true)
        {
            switch (Input.inputString)  //키입력
            {
                case "q":
                    if(gripPos.childCount <= 0f) //손에 아이템이 없을 때
                    {
                        itemRig = currItem.GetComponent<Rigidbody>();               //손의 위치로 아이템 발사
                        Vector3 destination = gripPos.position - currItem.position;
                        //itemRig.useGravity = true;
                        itemRig.AddForce(destination * 1f, ForceMode.Impulse);
                        currItem.Rotate(Vector3.right * 2f);
                    }
                    break;
            }
            if(currItem != null)
            {
                float dist = (gripPos.position - currItem.position).sqrMagnitude; //거리체크
                if (dist <= 0.5f)
                {
                    //itemRig.useGravity = false;
                    itemRig.velocity = Vector3.zero;             //가속도 초기화

                    currItem.position = gripPos.position;            //손에 붙음
                    currItem.rotation = gripPos.rotation;
                    currItem.SetParent(gripPos);                     //부모 갈아타기
                }
            }
            yield return null;
        }
    }*/
    /*void OnDrawGizmos()//씬뷰 체크용
    {
        Gizmos.color = Color.red;
        if (isItem)
        {
            Gizmos.DrawRay(transform.position, transform.forward * castHit.distance);
            Gizmos.DrawWireSphere(transform.position + transform.forward * castHit.distance, radius);
        }
        else
        {
            Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
        }
    }*/
}
