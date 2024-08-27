using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlay : MonoBehaviour
{
    public VideoPlayer video;

    void Start()
    {
        video.Pause();
    }

    public void PlayVideo()
    {
        video.Play();
    }

    public void PauseVideo()
    {
        video.Pause();
    }
}
