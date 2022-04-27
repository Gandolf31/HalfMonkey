using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDie : MonoBehaviour
{
    public bool playerDie = false;
    public bool objectDie = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            playerDie = true;
        }
        else if (other.collider.CompareTag("StunObject")) 
        {
            objectDie = true;
        }
    }
}
