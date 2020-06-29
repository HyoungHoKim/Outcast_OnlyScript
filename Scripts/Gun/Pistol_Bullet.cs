using System;
using System.Collections;
using System.Collections.Generic;
//using System.Management.Instrumentation;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using Valve.VR;

public class Pistol_Bullet : MonoBehaviour
{
    public float damage = 100.0f;
    public float speed = 10.0f;
    public float initialForce = 1.0f;
    public float lifeTime = 30.0f;
    public float lifeTimer = 0f;

    public GameObject bulletHole;

    public GameObject[] hitEffects = new GameObject[] { null };

    private void Start()
    {
        transform.parent = null;
    }


    private void Update()
    {
        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifeTime) Destroy(this);

        

        GetComponent<Rigidbody>().AddRelativeForce(0, 0, initialForce);
    }

    private void OnCollisionEnter(Collision coll)
    {
        Debug.Log(coll.transform.name);

        if (!coll.collider.CompareTag("PISTOL") && !coll.collider.CompareTag("projectile")) 
            Hit(coll);
    }

    void Hit(Collision coll)
    {
        Debug.Log(coll.transform.name);

        //Damage Logic
        coll.gameObject.SendMessageUpwards("ChangeHP", damage, SendMessageOptions.DontRequireReceiver);

        Debug.Log(coll.collider.tag);

        // Place the bullet hole in the scene
        if (bulletHole != null)
            Instantiate(bulletHole, coll.contacts[0].point, Quaternion.FromToRotation(Vector3.up * 0.01f, coll.contacts[0].normal));

        foreach (GameObject hitEffect in hitEffects)
        {
            if (hitEffect != null)
                Instantiate(hitEffect, coll.contacts[0].point, Quaternion.FromToRotation(Vector3.up, coll.contacts[0].normal));
        }

        if (coll.rigidbody)
        {
            coll.rigidbody.AddForce(coll.contacts[0].normal * 100.0f);
        }

        Destroy(gameObject);
    }
}
