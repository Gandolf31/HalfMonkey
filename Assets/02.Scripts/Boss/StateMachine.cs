using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected BossManager bossManager;

    virtual public void Enter(GameObject target)
    {
        bossManager = target.GetComponent<BossManager>();
    }
    virtual public void Execute(GameObject target) { }
    virtual public void Exit(GameObject target) { }
}