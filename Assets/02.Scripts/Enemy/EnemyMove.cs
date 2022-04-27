using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class EnemyMove : MonoBehaviourPunCallbacks,IPunObservable
{
    Rigidbody2D rigid;
    public int nextMove;
    public float movePower = 1f;
    public PhotonView photonView;

    private void Awake()
    {
        rigid = GetComponentInParent<Rigidbody2D>();
        Invoke("Think", 5f); //Think�Լ� 5�� �Ŀ� ����
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.4f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (raycast.collider == null) //raycast�� ����Ǵ°��� ������ ������ Ʋ� �����δ�.
        {
            nextMove = nextMove * (-1);
            CancelInvoke();
            Invoke("Think", 5);
        }
        
        photonView.RPC("Move", RpcTarget.All);
    }

    [PunRPC]
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero; //moveVelocity�� �ʱⰪ 0

        if(nextMove == -1) //nextMove�� ���� -1�϶� �������� �̵�
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3 (1,1,1);
        }
        else if(nextMove == 1) //nextMove�� ���� 1�϶� ���������� �̵�
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3 (-1,1,1);
        }
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void Think() //nextMove�� -1, 0, 1�� ���̰� �����ϰ� �����ǰ�, time�� 2�ʿ��� 5�ʷ� �����ϰ� �����ȴ�.
    {
        nextMove = Random.Range(-1, 2);
        float time = Random.Range(2f, 5f);
        Invoke("Think", time);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("Apple") || other.collider.CompareTag("StunObject"))
        {
            photonView.RPC("doDie", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void doDie()
    {
        Destroy(gameObject);
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
