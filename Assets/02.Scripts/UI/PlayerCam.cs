using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public GameObject Player;

    void Start()
    {
    }
    void Update()
    {
        Vector3 TargetPos = new Vector3(Player.transform.position.x, Player.transform.position.y, -10);
        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * 2f);
    }
}
