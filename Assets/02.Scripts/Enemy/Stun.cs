using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "StunObject")
        {
            Invoke("Stuning", 2f);
            gameObject.GetComponent<EnemyMove>().enabled = false;
        }
    }

    void Stuning()
    {
        Destroy(gameObject);
    }
}
