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
        //Enter���� �ִϸ��̼� �� �� �ް� Execute ������
    }
    public override void Execute(GameObject target)
    {
        base.Execute(target);
        //�� ������ �ڵ�
        //�ѹ� ����Ǹ� ���� �������� ����, �������� ����Ǹ� ������ �ݴ���Ͽ� ��ǥ�������� �̵�
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