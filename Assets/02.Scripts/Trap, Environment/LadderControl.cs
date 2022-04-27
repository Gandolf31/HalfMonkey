using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderControl : MonoBehaviour
{

    public Collider2D platColl;

    private void OnTriggerStay2D(Collider2D collision)
    {
        float inputY = Input.GetAxis("Vertical");
        if(inputY != 0)
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), platColl, true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), platColl, false);
    }
}
