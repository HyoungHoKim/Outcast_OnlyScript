using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockedDoor : MonoBehaviour
{
    public Image lockedIcon;
    public float currTime;
    public float maxTime;

    private void OnEnable()
    {
        lockedIcon.fillAmount = 1.0f;
    }
    private void Update()
    {
        currTime -= (1f * Time.deltaTime);
        lockedIcon.fillAmount = currTime / maxTime;
    }
}
