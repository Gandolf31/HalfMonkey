using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start() 
    { 
        text = GetComponent<Text>(); StartCoroutine(BlinkText()); 
    }

    public IEnumerator BlinkText() 
    {
        while (true) 
        { 
            text.text = "";
            yield return new WaitForSeconds(1f); 
            text.text = "Spacebar to SKIP>>"; 
            yield return new WaitForSeconds(1f); 
        } 
    }
}
