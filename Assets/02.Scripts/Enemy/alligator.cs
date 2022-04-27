using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alligator : MonoBehaviour
{
    Rigidbody2D rigid;
    bool onAlligator;
    float speed = 5f;
    Vector3 targetVector = new Vector3(18f, -6f, 0f);
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (onAlligator)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetVector, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onAlligator = true;
        }
    }
}
