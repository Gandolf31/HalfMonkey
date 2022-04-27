using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Rush : StateMachine
{
    Animator BossAnimator;
    PolygonCollider2D BossCollider;
    SpriteRenderer BossSprite;
    Transform BossTransform;
    Rigidbody2D BossRigidbody;
    public GameObject BossPosition;
    public GameObject TargetPosition;
    bool isArrive = false;

    public override void Enter(GameObject target)
    {
        base.Enter(target);
        BossAnimator = bossManager.BossAnimator;
        BossCollider = bossManager.BossCollider;
        BossRigidbody = bossManager.BossRigidbody;
        Debug.Log("Random Rush");
        BossAnimator.SetTrigger("Rush");
        //Enter에서 애니메이션 한 번 받고 Execute 들어가야함
    }
    public override void Execute(GameObject target)
    {
        base.Execute(target);
        //주 움직임 코드
        //한번 실행되면 일정 지점까지 가고, 다음번에 진행되면 방향을 반대로하여 목표지점까지 이동
        if (isArrive == false && gameObject.transform.position.x >= TargetPosition.transform.position.x)
        {
            transform.localScale = new Vector2(-0.5f, 0.5f);
            bossManager.ChangeState(BossState.Idle);
            isArrive = true;
        }
        else if(isArrive == true && gameObject.transform.position.x <= BossPosition.transform.position.x)
        {
            transform.localScale = new Vector2(0.5f, 0.5f);
            bossManager.ChangeState(BossState.Idle);
            isArrive = false;
        }
        
        if(isArrive)
        {
            BossRigidbody.AddForce(Vector2.left);
        }
        else
        {
            BossRigidbody.AddForce(Vector2.right);
        }
    }
    public override void Exit(GameObject target)
    {
        base.Exit(target);
        bossManager.BossExState = BossState.Rush;
        StopAllCoroutines();
    }
}