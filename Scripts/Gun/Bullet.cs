using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed = 10000f;
    public float time = 0;
    public Rigidbody bulletRig;
    
    private void Update() {

        bulletRig.AddForce(0,0,10000);    
        time += Time.deltaTime;
        if(time >5) {
            Destroy(bullet);
            time = 0;
        }
    }

}
