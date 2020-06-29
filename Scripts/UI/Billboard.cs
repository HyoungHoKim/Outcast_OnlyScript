using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{
    [Header("MyScript")]
    private Billboard billboard;

    [Header("Transform")]
    private Transform myTr;
    private Transform playerTr;

    
    private void OnEnable()
    {
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        billboard = this.GetComponent<Billboard>();
        myTr = this.GetComponent<Transform>();
        StartCoroutine(CameraCheck());
        StartCoroutine(LookPlayer());
    }
    IEnumerator CameraCheck()
    {
        while(billboard.isActiveAndEnabled)
        {
            yield return null;
            playerTr = Camera.main.transform;
        }        
    }

    IEnumerator LookPlayer()
    {
        while (billboard.isActiveAndEnabled)
        {
            yield return new WaitForSeconds(0.1f);
            if (myTr != null && playerTr != null)
            {
                myTr.LookAt(playerTr.position);
            }
        }
    }
}