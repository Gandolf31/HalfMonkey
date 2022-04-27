using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    Rigidbody2D rigid2D;
    BoxCollider2D boxCollider2D;
    Animator anim;
    public bool isGround;

    void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(Think());
    }

    void Update()
    {
        if(rigid2D.velocity.y == 0)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);
        int randomAction = Random.Range(0, 4);

        switch (randomAction)
        {
            case 0:
                StartCoroutine(Walk());
                break;
            case 1:
                StartCoroutine(Rush());
                break;
            case 2:
                StartCoroutine(Jump());
                break;
            case 3:
                StartCoroutine(Attack1());
                break;
            case 4:
                StartCoroutine(Idle());
                break;
        }
    }
    IEnumerator Walk()
    {
        anim.SetInteger("AnimState", 1);
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Think());
    }
    IEnumerator Rush()
    {
        anim.SetTrigger("Rush");
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Think());
    }
    IEnumerator Jump()
    {
        anim.SetTrigger("Jump");
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(Think());
    }
    IEnumerator Attack1()
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(Think());
    }
    IEnumerator Idle()
    {
        anim.SetInteger("AnimState", 0);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Think());
    }
}