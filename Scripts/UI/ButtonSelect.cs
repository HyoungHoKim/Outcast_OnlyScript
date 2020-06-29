using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelect : MonoBehaviour
{
    public Transform textPanel;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public Transform hudCanvas;

    public void PanelOn()
    {
        if (!textPanel.gameObject.activeSelf)
        {
            audioSource.clip = audioClips[0];
            audioSource.Play();

        }
        textPanel.gameObject.SetActive(true);
    }
    public void PanelOff()
    {
        textPanel.gameObject.SetActive(false);
    }
    public void ButtonClick()
    {
        //audioSource.clip = audioClips[1];
        audioSource.PlayOneShot(audioClips[1]);
        hudCanvas.gameObject.SetActive(false);
    }

    
}
