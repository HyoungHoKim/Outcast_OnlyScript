using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollider : MonoBehaviour
{
    public GateAccess[] gateAccess;
    private void OnCollisionEnter(Collision coll)
    {
        if(gateAccess[0] != null)
        {
            if (coll.collider.gameObject.CompareTag("ENEMY"))
            {
                for (int i = 0; i < gateAccess.Length; i++)
                {
                    gateAccess[i].enabled = true;
                    gateAccess[i].isTouched = true;
                }
            }
        }        
    }
}
