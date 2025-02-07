using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource SFXSource;

    public AudioClip backgroundMusic;
    public AudioClip doorClosing;
    public AudioClip deathSound;


    private void Start()
    {
        musicSource.loop = true;
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);//détruit pas le Player lorsqu'il change de scene.
        }
        else
        {
            Destroy(gameObject);//Si Instance n'est pas définit.
        }
    }
}
