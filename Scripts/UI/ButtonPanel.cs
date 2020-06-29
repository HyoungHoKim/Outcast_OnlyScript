using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanel : MonoBehaviour
{
    public Animator anim;
    private void OnEnable()
    {
        anim.SetTrigger("Selected");
    }
    private void OnDisable()
    {
        anim.SetTrigger("Selected");
    }
}
