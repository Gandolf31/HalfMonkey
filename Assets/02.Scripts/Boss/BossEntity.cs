using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum BossState
{
    //Boss Normal State
    Idle,
    //Boss Attack State
    Rush, Smash
}
public class BossEntity : MonoBehaviour
{
    public BossState BossCurrentState = BossState.Idle;
    public BossState BossExState;

    protected Idle BossIdle = null;
    protected Rush BossRush = null;
    protected Smash BossSmash = null;

    [Header("Random Action Percentage")]
    public float RandomSearchCycle;
    public float RandomRushPercentage;
    public float RandomSmashPercentage;

    //Boss Components
    [HideInInspector]
    public Animator BossAnimator;
    [HideInInspector]
    public PolygonCollider2D BossCollider;
    [HideInInspector]
    public SpriteRenderer BossSprite;
    [HideInInspector]
    public Rigidbody2D BossRigidbody;
    [HideInInspector]
    public Transform BossTransform;
}