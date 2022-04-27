using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushWood : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }
}
