using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Ray : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.right * 100f, Color.blue , 100.0f);
    }
}
