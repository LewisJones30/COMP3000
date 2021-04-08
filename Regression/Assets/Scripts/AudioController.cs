using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    AudioClip LosePowerMusic, FinalBossMusic, IngameMusic, MagicAttack, MeleeAttack;
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
        audioController.clip = LosePowerMusic;
        audioController.Play();
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
        audioController.clip = FinalBossMusic;
        audioController.Play();
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
        audioController.clip = IngameMusic;
        audioController.Play();
    }
    public void Stop()
    {
        audioController.Stop();
    }
    public void PlayMagicAttack()
    {
        if (audioController == null)
        {
            audioController = GetComponent<AudioSource>();
        }
        audioController.PlayOneShot(MagicAttack, 0.5f);
    }
    public void PlayMeleeAttack()
    {
        if (audioController == null)
        {
            audioController = GetComponent<AudioSource>();
        }
        audioController.PlayOneShot(MeleeAttack, 0.5f);
    }
}
