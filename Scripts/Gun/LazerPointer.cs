using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerPointer : MonoBehaviour
{
    bool isLazer = false;

    public float range = 9999.9f;
    public GameObject hitEffect;

    private void Update()
    {


        Ray ray = new Ray(this.transform.position, -this.transform.right);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            // Hit Effects         
            Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));       
        }

    }
}
