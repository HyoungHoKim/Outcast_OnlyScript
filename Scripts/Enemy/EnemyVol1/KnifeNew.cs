using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using RootMotion;
using RootMotion.Dynamics;


public class KnifeNew : MonoBehaviour
{
    
    public GameObject knife;
    public Rigidbody knifeRig;
    public SteamVR_Behaviour_Pose pose;
    public Collider kinfeColl;
    public PuppetMaster puppetMaster;
    public EnemyHealth EnemyHealth;
    public Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    
    {  
        velocity = pose.GetVelocity();
        if (velocity.magnitude > 1)
        {
            kinfeColl.isTrigger = true;
        }
        else
        {
            kinfeColl.isTrigger = false;
        }
       // Debug.Log(velocity.magnitude +"가능");
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ENEMYHEAD"))
        {
            if (velocity.magnitude > 3)
            {
                
                Debug.Log("3을 넘었습니다");
               EnemyHealth =  other.gameObject.GetComponentInParent<EnemyHealth>();
               puppetMaster = other.gameObject.GetComponentInParent<PuppetMaster>();
               puppetMaster.state = PuppetMaster.State.Dead;
               EnemyHealth.enemyHead.SetActive((false));
               EnemyHealth.enemyHeadRen.SetActive((false));
               EnemyHealth.EnemyHeadPart.SetActive(true);
               EnemyHealth.EnemyHeadPart.transform.parent = null;
                
                
            }
            
            
            
        }
        
    }
    

}
