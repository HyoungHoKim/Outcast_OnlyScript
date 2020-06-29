using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed;
    public float time = 0;

    private void Update()
    {
        bullet.transform.parent = null;
        bullet.transform.Translate(0,0,bulletSpeed);    
        //bullet.transform.Translate(0, 0, bulletSpeed);
        time += Time.deltaTime;
        if(time >5) {
            Destroy(bullet);
            time = 0;
        }
    }

}
