using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalRopegen : MonoBehaviour
{
    public GameObject Rope;
    public int ropeCnt;
    public Rigidbody2D pointRig;
    FixedJoint2D exJoint;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ropeCnt; i++)
        {
            FixedJoint2D curJoint = Instantiate(Rope, transform).GetComponent<FixedJoint2D>();
            curJoint.transform.localPosition = new Vector3(0, (i + 1) * -.5f, 0);
            if (i == 0)
                curJoint.connectedBody = pointRig;
            else
                curJoint.connectedBody = exJoint.GetComponent<Rigidbody2D>();
            exJoint = curJoint;
            if (i == ropeCnt - 1)
            {
                curJoint.GetComponent<Rigidbody2D>().mass = 5;
                curJoint.GetComponent<SpriteRenderer>().enabled = false;
                curJoint.GetComponent<CapsuleCollider2D>().enabled = false;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
