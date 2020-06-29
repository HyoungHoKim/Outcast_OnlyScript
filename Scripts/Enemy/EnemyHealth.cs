using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.Dynamics;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float DefaultHP;
    public PuppetMaster puppetMaster;
    public GameObject enemyHead;
    public GameObject enemyHeadRen;
    public GameObject EnemyHeadPart;
    

    [SerializeField]
    public float CurHP;

    // Start is called before the first frame update
    void Start()
    {
        CurHP = DefaultHP;
    }

    private void FixedUpdate()
    {
        if (CurHP <= 0)
        {
            
            puppetMaster.state = PuppetMaster.State.Dead;
            
        }
        
        
    }
}
