using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GhostPlayer : MonoBehaviourPunCallbacks , IPunObservable
{
    playerMove thePlayer;
    Rigidbody2D rigidbody;
    public Vector3 position;
    float speed = 5;
    public PhotonView photon;
    Vector3 notIsMinePosition;
   
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<playerMove>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        print(thePlayer.isDead);
        if (photon.IsMine)
        {
            if (thePlayer.isDead)
            {
                
                thePlayer.GetComponent<BoxCollider2D>().isTrigger = true;
                rigidbody.Sleep();
                position = transform.position;
                rigidbody.mass = 0;
                position.x += speed * Time.deltaTime * Input.GetAxisRaw("Horizontal");
                position.y += speed * Time.deltaTime * Input.GetAxisRaw("Vertical");

                transform.position = position;
            }
            else
            { 
                thePlayer.GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }
        else if ((transform.position - notIsMinePosition).sqrMagnitude >= 100)
        {
            transform.position = notIsMinePosition;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, notIsMinePosition, Time.deltaTime * 10);

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
            notIsMinePosition = (Vector3)stream.ReceiveNext();
        }
    }
}
