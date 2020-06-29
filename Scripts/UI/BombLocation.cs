using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLocation : MonoBehaviour
{
    [Header("Transform")]
    public Transform bomb;
    public QuestManager questManager;
    private Transform myTr;
    private BombLocation bombLocation;

    void Start()
    {
        myTr = this.GetComponent<Transform>();
        bombLocation = this.GetComponent<BombLocation>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BOMB"))
        {
            Destroy(other.gameObject);
            bomb.gameObject.SetActive(true);
            bombLocation.enabled = false;
            questManager.questInt += 1;
        }
    }
}
