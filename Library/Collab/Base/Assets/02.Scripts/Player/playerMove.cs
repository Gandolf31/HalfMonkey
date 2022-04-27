using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Cinemachine;
public class playerMove : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool             isLadder = false; // ��ٸ�
    public bool             isRope = false; // ����
    public float            MaxSpeed; // �ְ�ӵ�
    public float            JumpPower; // �����Ŀ�
    public float            Power;
    public int              jumpCount = 2; // ���� �� �� �ִ� Ƚ��
    private bool isJumping = false; //�����Ҷ� üũ
    public Rigidbody2D      rigid; // RigidBody ���

    private PlayerSensor    groundSensor;
    private PlayerSensor    wallSensorR1;
    private PlayerSensor    wallSensorR2;
    private PlayerSensor    wallSensorL1;
    private PlayerSensor    wallSensorL2;
    private bool            nowRope;
    private bool            isDash;
    private bool            isMove;
    private bool            isGrounded = false;
    private bool            isDead = false;

    FixedJoint2D            fixjoint;
    Rigidbody2D             ropeRigid;
    SpriteRenderer          spriteRenderer; // SpriteRenderer ���
    Animator                anim; // Animator ���

    // ����
    float                   distance;
    public PhotonView       view;
    public Image            NickName;
    public GameObject[]     transform_distance = new GameObject[2];
    Vector3                 notIsMinePosition;
    public CinemachineVirtualCamera CM;

    void Awake()
    {
        fixjoint = GetComponent<FixedJoint2D>(); // ���� ������ ����
        rigid = GetComponent<Rigidbody2D>(); // rigidbody ������Ʈ ȣ��
        spriteRenderer = GetComponent<SpriteRenderer>(); // Spriterenderer ������Ʈ ȣ��
        anim = GetComponent<Animator>(); // Animator ������Ʈ ȣ��
        view = GetComponent<PhotonView>();

        groundSensor = transform.Find("GroundSensor").GetComponent<PlayerSensor>();
        wallSensorR1 = transform.Find("WallSensorR1").GetComponent<PlayerSensor>();
        wallSensorR2 = transform.Find("WallSensorR2").GetComponent<PlayerSensor>();
        wallSensorL1 = transform.Find("WallSensorL1").GetComponent<PlayerSensor>();
        wallSensorL2 = transform.Find("WallSensorL2").GetComponent<PlayerSensor>();

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
            // ĳ���Ͱ� ��� ���� ��Ҵ��� Ȯ��
            if (!isGrounded && groundSensor.State())
            {
                isGrounded = true;
                // //m_animator.SetBool("Grounded", m_grounded);
            }
            if (isGrounded && !groundSensor.State())
            {
                isGrounded = false;
                //m_animator.SetBool("Grounded", m_grounded);
            }

            float InputX = Input.GetAxisRaw("Horizontal");
            float InputY = Input.GetAxisRaw("Vertical");

            //Jump
            if (Input.GetButtonDown("Jump") && jumpCount >= 0) //2�� ���� ����
            {
                if (jumpCount > 0)
                {
                    jumpCount--;
                    if (jumpCount > 2) return;

                    anim.SetBool("isJumping", true);
                    anim.SetBool("isRunning", false);
                    Jump();
                    isJumping = true;
                }

                if (fixjoint.connectedBody != null)
                {
                    fixjoint.connectedBody = null;
                    fixjoint.enabled = false;
                    nowRope = false;
                }
            }

            if (isJumping && rigid.velocity.y == 0) //������ üũ�Ͽ� �����ִϸ��̼� ����
            {
                jumpCount = 2;
                anim.SetBool("isJumping", false);
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

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isDash = true;
            }
          

        //Direction Sprite
        else if (InputX != 0)
        {
            view.RPC("Flip", RpcTarget.AllBuffered, InputX);
        }

            //Animation
            if (InputX != 0)       
            {
                isMove = true;
                anim.SetBool("isWalking", true);
            }
            else if (InputX == 0)
            {
                isMove = false;
                anim.SetBool("isWalking", false);
            }
            else if (rigid.velocity.x <= MaxSpeed)
            {
                anim.SetBool("isRunning", false);
            }
            else if (rigid.velocity.x > MaxSpeed)
            {
                anim.SetBool("isRunning", true);
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
            float InputX = Input.GetAxis("Horizontal");
            float InputY = Input.GetAxis("Vertical");

            float SlowDownSpeed = isMove ? 1.0f : 0.5f;
            rigid.velocity = new Vector2(InputX * MaxSpeed * SlowDownSpeed, rigid.velocity.y);

            if (isLadder && Mathf.Abs(InputY) > Mathf.Epsilon)
            {
                rigid.gravityScale = 0;
                rigid.velocity = new Vector2(rigid.velocity.x * 0.5f, MaxSpeed * InputY);
            }

            else if (isRope)
                rigid.AddForce(new Vector2(InputY * MaxSpeed, 0));

            else if (isDash)
                rigid.velocity = new Vector2(MaxSpeed * Power * InputX, rigid.velocity.y);
            
            if (transform_distance.Length == 2)
            {
                distance = Vector2.Distance(transform_distance[0].transform.position, transform_distance[1].transform.position);
                CM.m_Lens.OrthographicSize = Mathf.Lerp(CM.m_Lens.OrthographicSize, distance, Time.deltaTime);
                if (distance <= 4)
                    CM.m_Lens.OrthographicSize = 4f;
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
    [PunRPC]
    void Flip(float inputX)
    {
        print("Flip");
        transform.localScale = new Vector3(inputX, 1, 1);
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
        rigid.AddForce(new Vector2(dirc,1)*7, ForceMode2D.Impulse);

        Invoke("OffDamaged", 3);
    }

    void OffDamaged()
    {
        gameObject.layer = 10;
        //spriteRenderer.color = new Color(1, 1, 1, 1);
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
