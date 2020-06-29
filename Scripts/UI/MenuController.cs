using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void HelpMessege()
    {

    }
    public void RestartScene()
    {
            SceneManager.LoadScene("TestStage3");

        /*if (SceneManager.GetActiveScene().buildIndex == 2)
            SceneManager.LoadScene("04.InGame");*/
    }

    public void MainScene()
    {
        //SceneManager.LoadScene("00.Main");
    }
    public void GameQuit()
    {
        Application.Quit();
    }
}
