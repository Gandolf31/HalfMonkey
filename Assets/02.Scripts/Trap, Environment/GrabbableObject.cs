using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GrabbableObject : MonoBehaviourPunCallbacks, IPunObservable
{
    GameObject          player;
    CircleCollider2D    collider;
    Rigidbody2D         rigidbody;
    public PhotonView   photon;
    public GameObject   playerHands;
    public bool         isPlayerGrabbable;
    public bool         isThrowed = false;
    public bool         onGrab = false;
    bool                onTarget = false;
    
    Die                 isDie;
    Vector2             forceDir;

    void Start()
    {
        /* �浹 �ľ��� ����ϰ� �ϱ� ���� ���̾� Ȯ��
        GrabbableLayer = LayerMask.NameToLayer("Grabbable");
        PlayerLayer = LayerMask.NameToLayer("Player");
        */
        collider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();
        isDie = FindObjectOfType<Die>();
    }

    // Update is called once per frame
    void Update()
    {
        //photon.RPC("ToTrow", RpcTarget.AllBuffered);
        //playerHands = GameObject.FindGameObjectWithTag("Hand"); //�ϴ��� ������Ʈ���� �ҷ������� �߾�_�μ�
            //e Ű�� ���� ���� ���
        photon.RPC("ToThrow", RpcTarget.AllBuffered);

        if (isDie.isDying)
        {
            playerHands.transform.DetachChildren();
            rigidbody.isKinematic = false;
        }
    }

    [PunRPC]
    void ToThrow()
    {
        if (Input.GetKeyDown("e") && isPlayerGrabbable)
        {
            rigidbody.isKinematic = true;
            rigidbody.Sleep();
            collider.isTrigger = true;
            onGrab = true;
            //rigidbody.gravityScale = 0;
            transform.SetParent(playerHands.transform);
            transform.localPosition = Vector2.zero;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            isPlayerGrabbable = false;
          
        }
        //������ ��� ���� �� eŰ�� ���� ���� ������
        else if (Input.GetKeyDown("e") && !isPlayerGrabbable && !isThrowed && onGrab)
        {
            isThrowed = true;
            rigidbody.isKinematic = false;
            collider.isTrigger = false;
            rigidbody.IsAwake();
            //rigidbody.gravityScale = 1;
            playerHands.transform.DetachChildren();
            if (player.transform.localScale.x > 0)
            {
                rigidbody.AddForce(new Vector2(10, 0), ForceMode2D.Impulse);
                rigidbody.gravityScale = 0;
                Destroy(this.gameObject, 3.0f);
            }
            else if (player.transform.localScale.x < 0)
            {
                rigidbody.AddForce(new Vector2(-10, 0), ForceMode2D.Impulse);
                rigidbody.gravityScale = 0;
                Destroy(this.gameObject, 3.0f);
            }

            if (onTarget)
            {
                Destroy(this);
            }
            if(onGrab && isThrowed)
            {
                StartCoroutine(WaitSeconds());
            }
            gameObject.layer = 12;
        }
    }

    //collision �� ��� ����/�Ұ���
    /*private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") && other.collider.GetComponent<PhotonView>().IsMine)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerHands = player.transform.GetChild(1).gameObject;
            isPlayerGrabbable = true;
            Debug.Log("cd");
        }
    }*/
    //�±� Ȱ���ؼ� ���������� �����߾�_�μ�

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                player = other.gameObject;
                playerHands = player.transform.GetChild(1).gameObject;
                isPlayerGrabbable = true;
                Debug.Log("cd");
            }
        }
        else if (other.gameObject.CompareTag("ObjectCollider"))
        {
            onTarget = true;
        }
    }

    /*private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") && other.collider.GetComponent<PhotonView>().IsMine)
            isPlayerGrabbable = false;
    }*/

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerGrabbable = false;
        }
    }

    IEnumerator WaitSeconds()
    {
        yield return new WaitForSeconds(.1f);
        isThrowed = false;
        onGrab = false;
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

/*void OnCollisionExit2D(Collider2D other)
{
   if (other.gameObject == player)
       isPlayerGrab = false;
}
*/
}
