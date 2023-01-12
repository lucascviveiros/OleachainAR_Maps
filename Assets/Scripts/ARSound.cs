using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSound : MonoBehaviour
{
    [SerializeField]
    private AudioSource audio;
    [SerializeField]
    private AudioSource audio2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio()
    {
        audio.Play();
    }

    public void PlayAudioTheme()
    {
        audio2.Play();
    }

    public void StopAudios()
    {
        audio.Pause();
        audio2.Pause();
    }
}
