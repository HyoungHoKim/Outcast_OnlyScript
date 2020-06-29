using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Valve.VR;

public class ButtonEvent : MonoBehaviour
{
    
    public UnityEvent onButtonSelect;
    public UnityEvent ControllerTrigger;
    public bool isTrigger = false;
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HAND"))
        {            
            isTrigger = false;
        }
    }
    public IEnumerator ButtonPlay()
    {
        if(isTrigger)
        ControllerTrigger.Invoke();
        yield return null;
    }
    
}
