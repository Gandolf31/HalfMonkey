using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Cinemachine;
public class Movement2D : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool isLadder = false; // ��ٸ�
    public bool isRope = false; // ����
    public float MaxSpeed; // �ְ�ӵ�
    public float JumpPower; // �����Ŀ�
    public float Power;
    int jumpCount = 2; // ���� �� �� �ִ� Ƚ��
    private bool isJumping = false;

    private bool nowRope;

    FixedJoint2D fixjoint;
    Rigidbody2D rigid; // RigidBody ���
    Rigidbody2D ropeRigid;
    SpriteRenderer spriteRenderer; // SpriteRenderer ���
    Animator anim; // Animator ���

    float distance;
    public PhotonView view;
    public Image NickName;
    public GameObject[] transform_distance = new GameObject[2];
    public CinemachineVirtualCamera CM;
    Vector3 notIsMinePosition;


    void Awake()
    {
        fixjoint = GetComponent<FixedJoint2D>(); // ���� ������ ����
        rigid = GetComponent<Rigidbody2D>(); // rigidbody ������Ʈ ȣ��
        spriteRenderer = GetComponent<SpriteRenderer>(); // Spriterenderer ������Ʈ ȣ��
        anim = GetComponent<Animator>(); // Animator ������Ʈ ȣ��
        view = GetComponent<PhotonView>();

        NickName.color = view.IsMine ? Color.green : Color.red;

        if (view.IsMine)
        {
            CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;
            CM.m_Lens.OrthographicSize = 3f;
        }
        view.RPC("OnTarget", RpcTarget.AllBuffered);

        // �� ���� �ʱ�ȭ
        fixjoint.connectedBody = null;
        fixjoint.enabled = false;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
           
            float Hor = Input.GetAxisRaw("Horizontal");
            float InputY = Input.GetAxis("Vertical");
           
           
            //��ٸ�Ÿ��
            if (isLadder && Mathf.Abs(InputY) > Mathf.Epsilon)
            {
                rigid.gravityScale = 0;
                rigid.velocity = new Vector2(rigid.velocity.x * 0, MaxSpeed * InputY);
            }

            //Jump
            if (Input.GetButtonDown("Jump") && jumpCount >= 0)
            {
                if (jumpCount > 0)
                {
                    Debug.Log(jumpCount);
                    jumpCount--;
                    if (jumpCount > 2)

                        return;

                    Jump();
                    anim.SetBool("isJumping", true);
                    anim.SetBool("isRunning", false);
                    isJumping = true;
                }
                //���� ���� �ڵ�
                //rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);

                if (fixjoint.connectedBody != null)
                {
                    fixjoint.connectedBody = null;
                    fixjoint.enabled = false;
                    nowRope = false;
                }

            }

            else if (Input.GetKeyDown("e"))
            {
                if (isRope)
                {
                    fixjoint.connectedBody = ropeRigid;
                    fixjoint.enabled = true;
                    nowRope = true;
                }
            }

            //Direction Sprite
            else if (Input.GetButton("Horizontal"))
            {
                view.RPC("Flip", RpcTarget.AllBuffered, Hor);
            }


            //Animation
            if (Mathf.Abs(rigid.velocity.x) < 0.6)
            {
                anim.SetBool("isWalking", false);
            }
            else
            {
                anim.SetBool("isWalking", true);
            }

            if(isJumping && rigid.velocity.y == 0)
            {
                jumpCount = 2;
                anim.SetBool("isJumping", false);
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

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            //Move Speed
            float h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h * 2, ForceMode2D.Impulse);
            if (rigid.velocity.x > MaxSpeed) //Right MaxSpeed
                rigid.velocity = new Vector2(MaxSpeed, rigid.velocity.y);
            else if (rigid.velocity.x < MaxSpeed * (-1)) // Left MaxSpeed
                rigid.velocity = new Vector2(MaxSpeed * (-1), rigid.velocity.y);


            if (rigid.velocity.x != 0)
            {
                if (Input.GetKey(KeyCode.B))
                {
                    // �̺κ� �ڵ带 ���ؼ� �ϳ��� ���� �� ������
                    if (Input.GetAxisRaw("Horizontal") == 1)
                    {
                        if (rigid.velocity.x > MaxSpeed) //Right MaxSpeed
                            rigid.velocity = new Vector2(MaxSpeed * Power, rigid.velocity.y);
                        rigid.AddForce(Vector2.right * Power, ForceMode2D.Impulse);

                        anim.SetBool("isRunning", true);
                    }
                      else if(Input.GetAxisRaw("Horizontal") == -1) {
                        if (rigid.velocity.x < MaxSpeed * (-1))                   
                            rigid.velocity = new Vector2(MaxSpeed * (-1) * Power, rigid.velocity.y); //Left MaxSpeed
                            rigid.AddForce(Vector2.left * Power, ForceMode2D.Impulse);
                            anim.SetBool("isRunning", true);
                        
                    }
                   
                   
                }
                if (isRope)
                {
                    rigid.AddForce(new Vector2(h * MaxSpeed, 0));
                }
                else
                {
                    anim.SetBool("isRunning", false);
                    
                }
            }

            //Landing Platform
           
            if (transform_distance.Length == 2)
            {
                distance = Vector2.Distance(transform_distance[0].transform.position, transform_distance[1].transform.position);
                //print(distance);

                CM.m_Lens.OrthographicSize = Mathf.Lerp(CM.m_Lens.OrthographicSize, distance, Time.deltaTime);
                if (distance <= 4)
                {
                    CM.m_Lens.OrthographicSize = 4f;
                }
            }

            if (!isLadder)
                rigid.gravityScale = 1f;

        }

    }
   
  
    private void Jump()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, JumpPower);
    }

    [PunRPC]
    void OnTarget()
    {
        transform_distance = GameObject.FindGameObjectsWithTag("Player");
    }

    // �ø� �ٲ����, ���� �̵��� �ڿ���������� localscale�� �����ؾ� ��. �ٲ�
    [PunRPC]
    void Flip(float Hor)
    {
        print("Flip");
        //spriteRenderer.flipX = Hor == -1;
        transform.localScale = new Vector3(Hor, 1, 1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            OnDamaged(collision.transform.position);


    }

  

    // ��ٸ�, ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
            isLadder = true;
        if (other.CompareTag("Rope"))
        {
            ropeRigid = other.gameObject.GetComponent<Rigidbody2D>();
            isRope = true;
        }

        /*if (other.CompareTag("Floor"))
            anim.SetBool("isJumping", false);

        jumpCount = 2;*/
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
            isLadder = false;
        if (other.CompareTag("Rope"))
        {
            isRope = false;
            ropeRigid = null;
        }
    }

    void OnDamaged(Vector2 targetpos)
    {
        gameObject.layer = 11;

        //spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        int dirc = transform.position.x - targetpos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);
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
