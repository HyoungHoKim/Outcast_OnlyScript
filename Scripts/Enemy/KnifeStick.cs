using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeStick : MonoBehaviour
{

    public GameObject knife;
    public Rigidbody knifeRig;
    public GameObject enemy;
    public Transform stickPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ENEMY"))
        {
            knifeRig.isKinematic = true;
            knife.transform.parent = collision.transform;
            knife.transform.rotation = Quaternion.Euler(0, 0, 0);
            Vector3 offset = this.transform.position - stickPos.transform.position;
            knife.transform.position += offset;
        }
    }
}
