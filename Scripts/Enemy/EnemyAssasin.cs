using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAssasin : MonoBehaviour
{

    public GameObject EnemyA; 
    public float EnemyAssasinHp = 100f;
    public float lookRadius = 10f; //시야범위
    public Transform playerTr;
    public NavMeshAgent EnemyNav;
    public Animator EnemyAni;
    public GameObject leftLeg;
    public GameObject newleftLeg;
    public GameObject newleftLegBone;
    public Transform Rhand;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float distance = Vector3.Distance(playerTr.position, transform.position);

      

        if (distance <= lookRadius && distance > EnemyNav.stoppingDistance)
        {

            EnemyNav.SetDestination(playerTr.position);
            transform.LookAt(playerTr);

        }
        
    */

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("실행됨");
            Separation();
            
        }
    }
    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void RagdollOn()
    {
        EnemyAni.enabled = false;
    }

    public void RagdollOff()
    {
        EnemyAni.enabled = true;
    }

    public void Separation()
    {
        Instantiate(newleftLeg, Rhand);
        leftLeg.SetActive(false);
        EnemyAni.enabled = false;
        newleftLegBone.SetActive(false);



        Debug.Log("생성됨");
    }
    






}
