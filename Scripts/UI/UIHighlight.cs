using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHighlight : MonoBehaviour
{
    public Transform textPanel;
    public Animator anim;

    public void ButtonSelectedOn()
    {
        if(!textPanel.gameObject.activeSelf)
        textPanel.gameObject.SetActive(true);
        
    }
    public void ButtonSelectedOff()
    {
        if(textPanel.gameObject.activeSelf)
        textPanel.gameObject.SetActive(true);
    }
}
