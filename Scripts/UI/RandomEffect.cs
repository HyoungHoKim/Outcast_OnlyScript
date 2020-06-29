using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEffect : MonoBehaviour
{
    public Transform explosion01;
    public Transform explosion02;
    public Transform explosion03;
    public Transform explosion04;
    public Transform explosion05;
    public Transform explosion06;
    public Transform explosion07;
    public Transform explosion08;
    public Transform explosion09;

    private void Start()
    {
        Invoke("Play01", 1f);
        Invoke("Play02", 2f);
        Invoke("Play03", 3f);
        Invoke("Play04", 4f);
        Invoke("Play05", 5f);
        Invoke("Play06", 6f);
        Invoke("Play07", 7f);
        Invoke("Play08", 8f);
        Invoke("Play09", 9f);

    }
    void Play01()
    {
        explosion01.gameObject.SetActive(true);
    }
    void Play02()
    {
        explosion02.gameObject.SetActive(true);
    }
    void Play03()
    {
        explosion03.gameObject.SetActive(true);
    }
    void Play04()
    {
        explosion04.gameObject.SetActive(true);
    }
    void Play05()
    {
        explosion05.gameObject.SetActive(true);
    }
    void Play06()
    {
        explosion06.gameObject.SetActive(true);

    }
    void Play07()
    {
        explosion07.gameObject.SetActive(true);
    }
    void Play08()
    {
        explosion08.gameObject.SetActive(true);
    }
    void Play09()
    {
        explosion09.gameObject.SetActive(true);
    }
}
