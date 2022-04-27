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
        //Enter에서 애니메이션 한 번 받고 Execute 들어가야함
    }

    public override void Execute(GameObject target)
    {
        base.Execute(target);
    }

    public IEnumerator RandomAction()
    {
        Debug.Log("Random Search");
        yield return new WaitForSeconds(bossManager.RandomSearchCycle);

        float randomAction = Random.value; //0~1사이의 소수점
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