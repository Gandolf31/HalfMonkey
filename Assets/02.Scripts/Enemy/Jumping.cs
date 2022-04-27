using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour
{
    public GameObject enemy;
    public GameObject ElegatorPosition;
    public GameObject JumpingPosition;
    public float jumpPower;
    bool isEnter = false;
    Transform EnemyTransform;
    Rigidbody2D EnemyRigidbody;

    void Start()
    {
        EnemyTransform = enemy.GetComponent<Transform>();
        EnemyRigidbody = enemy.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isEnter)
        {
            EnemyRigidbody.AddForce(Vector2.up * jumpPower);
            if (EnemyTransform.position.y >= JumpingPosition.transform.position.y)
            {
                isEnter = false;
                EnemyRigidbody.AddForce(Vector2.down * jumpPower);
            }
        }
        else if (!isEnter)
        {
            if (EnemyTransform.position.y <= ElegatorPosition.transform.position.y)
            {
                EnemyTransform.position = ElegatorPosition.transform.position;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isEnter = true;
        }
    }
}
