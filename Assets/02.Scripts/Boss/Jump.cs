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
        //Enter���� �ִϸ��̼� �� �� �ް� Execute ������
    }
    public override void Execute(GameObject target)
    {
        base.Execute(target);
        //�� ������ �ڵ�
        //�ε�ȣ ���·� ���� ũ�� �Դٰ��� �ϴ� ���� �����ϰ� ����
        //�ѹ� ����Ǹ� ���� �������� ����, �������� ����Ǹ� ������ �ݴ���Ͽ� ��ǥ�������� �̵�
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
        //�����ϴ� �ִϸ��̼�

    }
    public override void Exit(GameObject target)
    {
        base.Exit(target);
        bossManager.BossExState = BossState.Rush;
        StopAllCoroutines();
    }
}
