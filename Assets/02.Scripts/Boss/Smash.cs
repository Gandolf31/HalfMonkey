using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Smash : StateMachine
{
    Animator BossAnimator;
    PolygonCollider2D BossCollider;
    SpriteRenderer BossSprite;
    Transform BossTransform;
    Rigidbody2D BossRigidbody;
    public GameObject BossPosition;
    public GameObject TargetPosition;
    public GameObject SmashPosition;
    public GameObject SmashPosition2;
    bool isArrive = false;
    bool isSmash = false;
  
    public override void Enter(GameObject target)
    {
        base.Enter(target);
        BossAnimator = bossManager.BossAnimator;
        BossCollider = bossManager.BossCollider;
        BossRigidbody = bossManager.BossRigidbody;
        Debug.Log("Random Smash");
        BossAnimator.SetTrigger("Jump");
        //Enter에서 애니메이션 한 번 받고 Execute 들어가야함
    }
    public override void Execute(GameObject target)
    {
        base.Execute(target);
        //주 움직임 코드
        //제자리 점프하는 느낌
        if (isSmash == true && isArrive == false && 
            gameObject.transform.position.y <= BossPosition.transform.position.y) //바닥에 가까워지면
        {
            bossManager.ChangeState(BossState.Idle);
            isSmash = false;
        }
        else if (isSmash == false && isArrive == true &&
            gameObject.transform.position.y >= SmashPosition2.transform.position.y) //위에 올라가면
        {
            isSmash = true;
                    }
        else if (isSmash == true && isArrive == false &&
            gameObject.transform.position.y <= TargetPosition.transform.position.y) //바닥에 가까워지면
        {
            bossManager.ChangeState(BossState.Idle);
            isSmash = false;
        }
        else if (isSmash == false && isArrive == false &&
            gameObject.transform.position.y >= SmashPosition.transform.position.y) //위에 올라가면
        {
            isSmash = true;
        }

        if(isSmash)
        {
            BossRigidbody.AddForce(Vector2.down * 10);
        }
        else
        {
            BossRigidbody.AddForce(Vector2.up * 10);
            if (BossRigidbody.velocity.y == 0)
                BossAnimator.SetFloat("AirSpeedY", BossRigidbody.velocity.y - Mathf.Epsilon); ;
        }
    }
    public override void Exit(GameObject target)
    {
        base.Exit(target);
        bossManager.BossExState = BossState.Smash;
        StopAllCoroutines();
    }
}