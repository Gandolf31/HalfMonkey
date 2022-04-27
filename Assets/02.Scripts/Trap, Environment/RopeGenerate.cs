using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class RopeGenerate : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject Rope;
    public GameObject RopeTop;
    public int ropeCnt;
    public Rigidbody2D pointRig;
    FixedJoint2D exJoint;
    public PhotonView photonview;

    NetworkManager network;

    // Start is called before the first frame update
    void Start()
    {
        network = FindObjectOfType<NetworkManager>();
        if (network.onCreate)
        {
            for (int i = 0; i < ropeCnt; i++)
            {
                photonview.RPC("RopeManager", RpcTarget.AllBuffered, i);
            }
        }
    }

    [PunRPC]
    void RopeManager(int i)
    {
        if (!photonview.IsMine)
        {
            //FixedJoint2D curJoint = Instantiate(Rope, transform).GetComponent<FixedJoint2D>();
            FixedJoint2D curJoint = PhotonNetwork.Instantiate("Rope", RopeTop.transform.position, Quaternion.identity).
                        GetComponent<FixedJoint2D>();
            curJoint.transform.localPosition = new Vector3(RopeTop.transform.position.x, RopeTop.transform.position.y + (i + 1) * -.5f, 0);
            if (i == 0)
                curJoint.connectedBody = pointRig;
            else
                curJoint.connectedBody = exJoint.GetComponent<Rigidbody2D>();
            exJoint = curJoint;
            if (i == ropeCnt - 1)
            {
                curJoint.GetComponent<Rigidbody2D>().mass = 5;
                curJoint.GetComponent<SpriteRenderer>().enabled = false;
                curJoint.GetComponent<CapsuleCollider2D>().enabled = false;
            }

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