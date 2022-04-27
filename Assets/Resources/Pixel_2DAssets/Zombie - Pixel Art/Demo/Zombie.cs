using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {

    [SerializeField] float      m_speed = 1.0f;

    FixedJoint2D fixjoint;
    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private SpriteRenderer      m_rend2d;
    private bool                m_isDead = false;

    void Start () {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_rend2d = GetComponent<SpriteRenderer>();
    }

    void Update() {
        float inputX = 0;
        float inputY = 0;

        if (!m_isDead)
        {
            inputX = Input.GetAxis("Horizontal");
            inputY = Input.GetAxis("Vertical");
        }
        if (inputX > 0 && inputY == 0)
            //transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            m_rend2d.flipX = true;
        else if (inputX < 0)
            //transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            m_rend2d.flipX = false;
        // Move       
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
        //사다리타기, 애니메이션 추가
        if (isLadder && !m_isDead && Mathf.Abs(inputY) > Mathf.Epsilon)
        {
            m_body2d.gravityScale = 0;
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_speed * inputY);
        }
        //Walk
        else if (Mathf.Abs(inputX) > Mathf.Epsilon && !m_isDead && inputY == 0)
            m_animator.SetInteger("AnimState", 1);
        //Idle
        else
        {
            m_animator.SetInteger("AnimState", 0);
        }
        if (!isLadder)
            m_body2d.gravityScale = 1f; 

    }

    public bool isLadder = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
            isLadder = true;            
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
            isLadder = false;
    }
}
