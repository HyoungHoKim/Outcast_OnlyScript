using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DroneItem : MonoBehaviour
{
    public Transform ammoTr;
    public Transform pistolTr;
    public Transform pMagazineTr;
    public Transform refMagazineTr;
    public Transform knifeTr;
    public Transform BombTr;
    public List<Transform> ammo;
    private Rigidbody ammoRig;

    public void CreatePistol()
    {
        ObjectCheck();      //이전에 생성했던 아이템 삭제
        var obj = Instantiate(pistolTr
                                    , ammoTr.position
                                    , ammoTr.rotation);
        ammoRig = obj.GetComponent<Rigidbody>();
        ammoRig.useGravity = false;
        ammo.Add(obj);
    }

    public void CreateMagazine()
    {
        ObjectCheck();
        var obj = Instantiate(pMagazineTr
                            , ammoTr.position
                            , ammoTr.rotation);
        ammoRig = obj.GetComponent<Rigidbody>();

        ammoRig.useGravity = false;
        ammo.Add(obj);
    }

    public void CreateReflectionMG()
    {
        ObjectCheck();
        var obj = Instantiate(refMagazineTr
                            , ammoTr.position
                            , ammoTr.rotation);
        ammoRig = obj.GetComponent<Rigidbody>();
        ammoRig.useGravity = false;
        ammo.Add(obj);
    }

    public void CreateKnife()
    {
        ObjectCheck();
        var obj = Instantiate(knifeTr
                            , ammoTr.position
                            , ammoTr.rotation);
        ammoRig = obj.GetComponent<Rigidbody>();
        ammoRig.useGravity = false;
        ammo.Add(obj);
    }

    public void CreateBomb()
    {
        ObjectCheck();
        var obj = Instantiate(BombTr
                            , ammoTr.position
                            , ammoTr.rotation);
        ammoRig = obj.GetComponent<Rigidbody>();
        ammoRig.useGravity = false;
        ammo.Add(obj);
    }

    public void ObjectCheck()      //이전에 생성했던 아이템 삭제
    {
        if (ammo.Count >= 1)
        {
            var obj = ammo[0];

            //Debug.Log(ammo[0].name);

            if (obj.transform.parent == null)
                Destroy(obj.gameObject);

            ammo.Clear();
        }
    }

}
