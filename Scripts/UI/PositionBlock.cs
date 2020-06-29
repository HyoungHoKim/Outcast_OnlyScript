using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionBlock : MonoBehaviour
{
    Transform a;
    // Start is called before the first frame update
    void Start()
    {
        a = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        a.position = Vector3.zero;
    }
}
