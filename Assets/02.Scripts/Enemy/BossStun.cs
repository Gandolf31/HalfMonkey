using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStun : MonoBehaviour
{
    int StunCount = 0;
    bool isStun = false;
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "StunObject")
        {
            isStuning();
            Debug.Log(StunCount);
        }
    }
    void Update()
    {
        if(isStun)
        {
            Invoke("Stuning", 2f);
            gameObject.GetComponent<BossManager>().enabled = false;
        }
    }

    void isStuning()
    {
        StunCount++;
        if (StunCount == 3)
        {
            isStun = true;
        }
    }

    void Stuning()
    {
        Destroy(gameObject);
    }
}
