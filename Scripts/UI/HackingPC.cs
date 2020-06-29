using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingPC : MonoBehaviour
{
    public QuestManager questManager;
    private AudioSource audioSource;
    public Transform visualCode;
    public Transform marker;
    private HackingPC hackingPC;
    public float maxTime = 5f;
    private float currTime;
    public bool isHacking = false;
    public bool isHacked = false;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        hackingPC = this.GetComponent<HackingPC>();
    }
    private void Update()
    {
        if (isHacking)
        {
            if (!visualCode.gameObject.activeSelf)
                audioSource.Play();

            visualCode.gameObject.SetActive(true);
            currTime += 1f * Time.deltaTime;
            if (currTime >= maxTime) //일정 시간이 되면
            {
                audioSource.Stop();
                visualCode.gameObject.SetActive(false);
                marker.gameObject.SetActive(false);
                questManager.questInt += 1;
                currTime = 0f;
                isHacking = false;
                //isHacked = true;
                Destroy(hackingPC);
            }
        }        
    }
}
