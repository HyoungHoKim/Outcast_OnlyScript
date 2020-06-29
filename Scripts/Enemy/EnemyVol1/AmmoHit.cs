using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoHit : MonoBehaviour
{
    public GameObject bullet;
    public Transform playerHead;
    public Rigidbody bulletRig;
    public Transform cube;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*
    void Update()
    {
        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Vector3 rayDir = Camera.main.transform.forward;
            
        Debug.DrawRay(cube.transform.position, cube.transform.position * 10, Color.red);    
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Instantiate(bullet, playerHead.transform);


        }
        
    }
    */
}
