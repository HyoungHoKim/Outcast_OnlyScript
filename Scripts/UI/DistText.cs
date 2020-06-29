using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistText : MonoBehaviour
{
    private Transform playerTr;
    public Text distTxt;
    private Vector3 originScale;
    private int dist;
    void Start()
    {
        //playerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();
        originScale = transform.localScale;
        StartCoroutine(DistantCalculator());
        
    }
    IEnumerator DistantCalculator()
    {
        while (this.gameObject.activeSelf)
        {
            var heading = playerTr.position - this.transform.position;
            transform.localScale = originScale * heading.magnitude * 0.1f;
            dist = Mathf.RoundToInt(heading.magnitude);
            distTxt.text = dist.ToString() + "m";
            yield return null;
        }     
    }
}
