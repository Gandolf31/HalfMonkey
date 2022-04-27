using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : StateMachine
{
    Animator BossAnimator;
    PolygonCollider2D BossCollider;
    SpriteRenderer BossSprite;
    Transform BossTransform;

    public override void Enter(GameObject target)
    {
        base.Enter(target);
        BossAnimator = bossManager.BossAnimator;
        BossCollider = bossManager.BossCollider;
        StartCoroutine("RandomAction");
        BossAnimator.SetTrigger("isIdle");
        //Enter���� �ִϸ��̼� �� �� �ް� Execute ������
    }

    public override void Execute(GameObject target)
    {
        base.Execute(target);
    }

    public IEnumerator RandomAction()
    {
        Debug.Log("Random Search");
        yield return new WaitForSeconds(bossManager.RandomSearchCycle);

        float randomAction = Random.value; //0~1������ �Ҽ���
        if(randomAction <= bossManager.RandomRushPercentage)
        {
            bossManager.ChangeState(BossState.Rush);
        }
        else if(randomAction < bossManager.RandomSmashPercentage)
        {
            bossManager.ChangeState(BossState.Smash);
        }
    }

    public override void Exit(GameObject target)
    {
        base.Exit(target);
        bossManager.BossExState = BossState.Idle;
        StopAllCoroutines();
    }
}