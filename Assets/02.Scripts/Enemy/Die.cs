using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Die : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject player, pos, Object, ObjectPos;
    float timeSpan;
    float checkTime;
    public bool isDying = false;
    playerMove playerMove;
    FallDie theFall;
    void Start()
    {
        timeSpan = 0.0f;
        checkTime = 5.0f;
        playerMove = FindObjectOfType<playerMove>();
        theFall = FindObjectOfType<FallDie>();
    }

    void Update()
    {
        print(timeSpan);
        print(theFall.objectDie);
        player = GameObject.FindGameObjectWithTag("Player");
        Object = GameObject.FindGameObjectWithTag("StunObject");
        pos = GameObject.Find("SpawnPoint");
        ObjectPos = GameObject.Find("SpawnPoint1");
        if (isDying || theFall.playerDie)
        {   
            timeSpan += Time.deltaTime;
            if (timeSpan > checkTime)
            {
                player.gameObject.transform.position = pos.transform.position;
                timeSpan = 0;
                isDying = false;
                theFall.playerDie = false;
                playerMove.isDead = false;
            }
        }
        else if (theFall.objectDie) 
        {
            timeSpan += Time.deltaTime;
            if (timeSpan > checkTime)
            {
                Object.gameObject.transform.position = ObjectPos.transform.position;
                timeSpan = 0;
                theFall.objectDie = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Player" && coll.gameObject.GetComponent<PhotonView>().IsMine)
        {
            isDying = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }
}
