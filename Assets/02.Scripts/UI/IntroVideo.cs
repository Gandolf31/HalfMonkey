using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroVideo : MonoBehaviour
{
    public GameObject introVideo;
    public GameObject onGame;
    public VideoPlayer video;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            introVideo.SetActive(false);
            onGame.SetActive(true);
        }

        if (!video.isPlaying && video.time > 0)
        {
            introVideo.SetActive(false);
            onGame.SetActive(true);
        }
    }
}
