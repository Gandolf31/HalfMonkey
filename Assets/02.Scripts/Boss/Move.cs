using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Move : StateMachine
{
    Animator BossAnimator;
    PolygonCollider2D BossCollider;
    public override void Enter(GameObject target)
    {
        base.Enter(target);
        BossAnimator = bossManager.BossAnimator;
        BossCollider = bossManager.BossCollider;
    }
    public override void Execute(GameObject target)
    {
        base.Execute(target);
        //주된 움직임 코드
    }
    public override void Exit(GameObject target)
    {
        base.Exit(target);
    }
}