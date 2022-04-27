using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FixWeb : MonoBehaviour
{
    public GameObject player;
    public GameObject web;
    private Vector3 enteredTransform;
    bool isFixed = false;
    float timeSpan;
    float checkTime;

    void Start()
    {
        timeSpan = 0f;
        checkTime = 5f;
    }
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (isFixed)
        {
            //player.gameObject.transform.position = web.transform.position;
            player.gameObject.transform.position = enteredTransform;
            timeSpan += Time.deltaTime;
            if(timeSpan >= checkTime)
            {
                timeSpan = 0;
                isFixed = false;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && other.GetComponent<PhotonView>().IsMine)
        {
            enteredTransform = player.transform.position;
            Fixing();
        }
    }

    void Fixing()
    {
        if(isFixed)
        {
            return;
        }
        isFixed = true;
    }
}
