using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossManager : BossEntity
{
    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        //Boss Components
        BossAnimator = this.gameObject.GetComponent<Animator>();
        BossCollider = this.gameObject.GetComponent<PolygonCollider2D>();
        BossSprite = this.gameObject.GetComponent<SpriteRenderer>();
        BossRigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        BossTransform = this.gameObject.GetComponent<Transform>();

        //Boss StateMachine
        BossIdle = this.gameObject.GetComponent<Idle>();
        BossRush = this.gameObject.GetComponent<Rush>();
        BossSmash = this.gameObject.GetComponent<Smash>();

        BossIdle.Enter(this.gameObject);
    }
    private void Update()
    {
        switch(BossCurrentState)
        {
            case BossState.Idle:
                BossIdle.Execute(this.gameObject);
                break;
            case BossState.Rush:
                BossRush.Execute(this.gameObject);
                break;
            case BossState.Smash:
                BossSmash.Execute(this.gameObject);
                break;
        }
    }
    public void ChangeState(BossState newState)
    {
        //Exit State
        switch(BossCurrentState)
        {
            case BossState.Idle:
                BossIdle.Exit(this.gameObject);
                break;
            case BossState.Rush:
                BossRush.Exit(this.gameObject);
                break;
            case BossState.Smash:
                BossSmash.Exit(this.gameObject);
                break;
        }

        //Change State
        BossCurrentState = newState;

        //EnterState
        switch(BossCurrentState)
        {
            case BossState.Idle:
                BossIdle.Enter(this.gameObject);
                break;
            case BossState.Rush:
                BossRush.Enter(this.gameObject);
                break;
            case BossState.Smash:
                BossSmash.Enter(this.gameObject);
                break;
        }
    }
}