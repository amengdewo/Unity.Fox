using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioSource;
    public AudioSource bgmSource;

    [SerializeField]
    private AudioClip jumpClip, hurtClip, cherryClip;

    void Awake()
    {
        instance = this;
    }

    public void JumpSound()
    {
        audioSource.PlayOneShot(jumpClip);
    }

    public void HurtSound()
    {
        audioSource.PlayOneShot(hurtClip);
    }

    public void CherrySound()
    {
        audioSource.PlayOneShot(cherryClip);
    }
}
