using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class EnemyController1 : MonoBehaviour
{
    public float lookRadius = 10f;
    public Transform playerTr;
    public NavMeshAgent EnemyNav;
    public Animator EnemyAni;
    public GameObject bullet;
    public float time;
    public Transform firePos;
    public float speed;
    private float waitTime;
    public float startWaitTime;
    public Transform[] moveSpot;
    public int randomSpot;
    public float enemyHp = 100;

    // Start is called before the first frame update
    void Start() {
        waitTime = startWaitTime;

        randomSpot = UnityEngine.Random.Range(0, moveSpot.Length);

    }

    void Update() {
        float distance = Vector3.Distance(playerTr.position, transform.position);

        if (distance > 10) {

            transform.position = Vector3.MoveTowards(transform.position, moveSpot[randomSpot].position, speed * Time.deltaTime);
            transform.LookAt(moveSpot[randomSpot]);
            EnemyAni.SetTrigger("IsWalk");

            if (Vector3.Distance(transform.position, moveSpot[randomSpot].position) < 0.2f) {
                if (waitTime <= 0) {
                    randomSpot = UnityEngine.Random.Range(0, moveSpot.Length);
                    waitTime = startWaitTime;

                }
                else {
                    waitTime -= Time.deltaTime;
                }

            }
        }
       

        if (distance <= lookRadius && distance >EnemyNav.stoppingDistance) {
           
            EnemyNav.SetDestination(playerTr.position);
            EnemyAni.SetTrigger("IsRun");
            transform.LookAt(playerTr);
        }
        if (distance <= EnemyNav.stoppingDistance) {
            
            EnemyAni.SetTrigger("Close");
            time += Time.deltaTime;

            if(time > 2f) {
                Instantiate(bullet,firePos);
                time = 0;
            }
          
          
        }
        if (enemyHp <= 0) {
            EnemyAni.SetTrigger("Die");
            transform.Translate(0, 0, 0);
            EnemyNav.speed = 0;
            speed = 0;
            Destroy(gameObject,2);
        }



    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

 
}
