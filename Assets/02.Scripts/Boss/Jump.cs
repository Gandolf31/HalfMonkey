using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jump : StateMachine
{
    Animator BossAnimator;
    PolygonCollider2D BossCollider;
    SpriteRenderer BossSprite;
    Transform BossTransform;
    Rigidbody2D BossRigidbody;
    public GameObject BossPosition;
    public GameObject TargetPosition;
    public GameObject JumpPosition;
    bool isArrive = false;
    bool isJump = false;

    public override void Enter(GameObject target)
    {
        base.Enter(target);
        BossAnimator = bossManager.BossAnimator;
        BossCollider = bossManager.BossCollider;
        BossTransform = bossManager.BossTransform;
        BossRigidbody = bossManager.BossRigidbody;
        //Enter에서 애니메이션 한 번 받고 Execute 들어가야함
    }
    public override void Execute(GameObject target)
    {
        base.Execute(target);
        //주 움직임 코드
        //부등호 형태로 맵을 크게 왔다갔다 하는 것을 구현하고 싶음
        //한번 실행되면 일정 지점까지 가고, 다음번에 진행되면 방향을 반대로하여 목표지점까지 이동
        if (isJump == false && isArrive == false &&
            gameObject.transform.position.x <= JumpPosition.transform.position.x &&
            gameObject.transform.position.y >= JumpPosition.transform.position.y)
        {
            BossTransform.rotation = Quaternion.Euler(0, 0, -45);
            bossManager.ChangeState(BossState.Idle);
            isJump = true;
        }
        else if (isJump == true && isArrive == false &&
            gameObject.transform.position.x >= JumpPosition.transform.position.x &&
            gameObject.transform.position.y >= JumpPosition.transform.position.y)
        {
            BossTransform.rotation = Quaternion.Euler(0, 0, 0);
            bossManager.ChangeState(BossState.Idle);
            isJump = false;
        }
        else if (isJump == true && isArrive == true &&
            gameObject.transform.position.x >= JumpPosition.transform.position.x &&
            gameObject.transform.position.y <= JumpPosition.transform.position.y)
        {
            BossTransform.rotation = Quaternion.Euler(0, 0, -45);
            bossManager.ChangeState(BossState.Idle);
            isJump = false;
        }
        else if (isJump == false && isArrive == true &&
            gameObject.transform.position.x <= JumpPosition.transform.position.x &&
            gameObject.transform.position.y <= JumpPosition.transform.position.y)
        {
            BossTransform.rotation = Quaternion.Euler(0, 0, 0);
            bossManager.ChangeState(BossState.Idle);
            isJump = true;
        }

        if (isJump && !isArrive)
        {
            BossRigidbody.AddForce(Vector2.down);
            BossRigidbody.AddForce(Vector2.right);
        }
        else if(!isJump && !isArrive)
        {
            BossRigidbody.AddForce(Vector2.up);
            BossRigidbody.AddForce(Vector2.right);
        }
        else if(!isJump && isArrive)
        {
            BossRigidbody.AddForce(Vector2.up);
            BossRigidbody.AddForce(Vector2.left);
        }
        else if(isJump && isArrive)
        {
            BossRigidbody.AddForce(Vector2.down);
            BossRigidbody.AddForce(Vector2.left);
        }
        //점프하는 애니메이션

    }
    public override void Exit(GameObject target)
    {
        base.Exit(target);
        bossManager.BossExState = BossState.Rush;
        StopAllCoroutines();
    }
}
