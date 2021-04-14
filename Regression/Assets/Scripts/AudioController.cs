using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    AudioClip LosePowerMusic, FinalBossMusic, IngameMusic, MagicAttack, MeleeAttack, GolemAttack, GolemDeath, MeleeMagicAttack, MeleeMagicDeath, MegaTrollAttack, MegaTrollDeath;
    AudioSource audioController;


    public void PlayMusic(int MusicID)
    {
        if (audioController == null)
        {
            audioController = GetComponent<AudioSource>();
        }
        audioController.Stop();
        audioController.volume = 1.0f;
        audioController.loop = true;
        //Music ID's start from zero.
        switch (MusicID)
        {
            case 0:
                {
                    audioController.clip = IngameMusic;
                    break;
                }
            case 1:
                {
                    audioController.clip = LosePowerMusic;
                    break;
                }
            case 2:
                {
                    audioController.clip = FinalBossMusic;
                    break;
                }

        }
        audioController.Play();
    }
    public void Stop()
    {
        audioController.Stop();
    }

    public void PlaySoundEffect(int SoundEffectID)
    {
        if (audioController == null)
        {
            audioController = GetComponent<AudioSource>();
        }
        switch (SoundEffectID)
        {
            
            case 0: //Melee Orc & Magic Ghoul attack sound
                {
                    audioController.PlayOneShot(MeleeMagicAttack);
                    break;
                }
            case 1: //Melee Orc & Magic Ghoul Death sound
                {
                    audioController.PlayOneShot(MeleeMagicDeath);
                    break;
                }
            case 2: //Mega Troll attack sound
                {
                    audioController.PlayOneShot(MegaTrollAttack);
                    break;
                }
            case 3: //Mega troll death sound
                {
                    audioController.PlayOneShot(MegaTrollDeath);
                    break;
                }
            case 4: //Golem attack sound
                {
                    audioController.PlayOneShot(GolemAttack);
                    break;
                }
            case 5: //Golem death sound.
                {
                    audioController.PlayOneShot(GolemDeath);
                    break;
                }
            case 6: //Magic projectile sound
                {
                    audioController.PlayOneShot(MagicAttack);
                    break;
                }
            case 7: //Melee projectile sound.
                {
                    audioController.PlayOneShot(MeleeAttack);
                    break;
                }
            default:
                {
                    break;
                }
        }
           
    }
}
