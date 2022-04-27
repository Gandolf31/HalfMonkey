using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckTime : MonoBehaviour
{
    public Text text_Timer;
    public float Seconds;
    public float Minute;
    void Update()
    {
        Seconds += Time.deltaTime;
        if (Seconds > 59)
        {
            Minute += 1;
            Seconds = 0;
        }
        text_Timer.text = "시간 : " + Mathf.Round(Minute) + "분" + Mathf.Round(Seconds) + "초";
    }
}
