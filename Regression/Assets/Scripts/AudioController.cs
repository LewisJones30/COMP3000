using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    AudioClip LosePowerMusic, FinalBossMusic, IngameMusic;
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
        audioController.volume = 1.0f;
        audioController.loop = true;
        audioController.PlayOneShot(LosePowerMusic);
    }
    public void PlayFinalBossMusic()
    {
        if (audioController == null)
        {
            audioController = GetComponent<AudioSource>();
        }
        audioController.Stop();
        audioController.volume = 1.0f;
        audioController.loop = true;
        audioController.PlayOneShot(FinalBossMusic);
    }
    public void PlayInGameMusic()
    {
        if (audioController == null)
        {
            audioController = GetComponent<AudioSource>();
        }
        audioController.Stop();
        audioController.volume = 0.3f;
        audioController.loop = true;
        audioController.PlayOneShot(IngameMusic);
    }
    public void Stop()
    {
        audioController.Stop();
    }
}
