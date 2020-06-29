using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public Transform exitPos;
    public int questInt = 0;
    public bool questSuccess;

    void Start()
    {
        
    }

    void Update()
    {
        if(questInt >= 5)
        {
            SceneManager.LoadScene("Build01");
            //if(!exitPos.gameObject.activeSelf)
            //    exitPos.gameObject.SetActive(true);
        }
    }
}
