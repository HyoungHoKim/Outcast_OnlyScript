using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateAccess : MonoBehaviour
{
    public bool isHacking = false;
    public bool isHacked = false;
    public bool isTouched = false;
    public Transform hackDisplay;
    public Rigidbody doorRig;
    private AudioSource audioSource;


    public float maxTime = 5f;
    private float currTime;

    private MeshRenderer meshRenderer;
    private GateAccess gateAccess;


    private void OnEnable()
    {
        audioSource = this.GetComponent<AudioSource>();
        meshRenderer = this.GetComponent<MeshRenderer>();
        gateAccess = this.GetComponent<GateAccess>();
    }
    private void Update()
    {
        if (isHacking)
        {
            if (!hackDisplay.gameObject.activeSelf)
            {
                if(audioSource.clip != null)
                    audioSource.Play();
            }
            hackDisplay.gameObject.SetActive(true);
            currTime += 1f * Time.deltaTime;
            if (currTime >= maxTime) //일정 시간이 되면
            {
                audioSource.Stop();
                hackDisplay.gameObject.SetActive(false);
                meshRenderer.material.SetColor("_EmissionColor", Color.cyan * 1.5f);
                doorRig.isKinematic = false;
                currTime = 0f;      //시간 초기화
                Destroy(gateAccess);
                
            }
        }
        if (isTouched)
        {
            meshRenderer.material.SetColor("_EmissionColor", Color.cyan * 1.5f);
            doorRig.isKinematic = false;
            Destroy(gateAccess);
        }
    }
}
