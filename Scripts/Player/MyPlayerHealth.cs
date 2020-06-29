using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerHealth : MonoBehaviour
{
    public float DefaultHP;

    [SerializeField]
    private float CurHP;

    private float CurTime;
    private float WaitTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        CurHP = DefaultHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurHP < DefaultHP)
        {
            CurTime += Time.deltaTime;

            Debug.Log(CurTime);

            if(CurTime >= WaitTime)
            {
                if (CurHP < DefaultHP) CurHP += 0.01f;
                else CurHP = DefaultHP;
            }
        }

    }

    void ChangeHP(float damage)
    {
        CurHP -= damage;
        CurTime = 0;
    }

}
