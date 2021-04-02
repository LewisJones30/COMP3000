using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    AudioClip losePowerMusic, finalBossMusic;
    AudioSource audioController;
    void Start()
    {
        
    }

    // Update is called once per frame

    public void PlayLosePowerMusic()
    {
        if (audioController == null)
        {
            audioController = GetComponent<AudioSource>();
        }
        audioController.Stop();
        audioController.loop = true;
        audioController.PlayOneShot(losePowerMusic);
    }
    public void PlayFinalBossMusic()
    {
        if (audioController == null)
        {
            audioController = GetComponent<AudioSource>();
        }
        audioController.Stop();
        audioController.loop = true;
        audioController.PlayOneShot(finalBossMusic);
    }
    public void Stop()
    {

    }
}
