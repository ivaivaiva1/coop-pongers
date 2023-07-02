using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioController : MonoBehaviour
{

    public static AudioController Instance;
 
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    void Awake() 
    {
        if(Instance == null)
        {
           Instance = this;
           DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start() 
    {
        PlayMusic("Menu");
    }

    public void PlayMusic(string name)
    {
       Sound s = Array.Find(musicSounds, x=> x.name == name);
       if(s == null)
       {
          Debug.Log("Sound Not Found");
       } else
       {
          musicSource.clip = s.clip;
          musicSource.Play();
       }
    }

    public void PlaySFX(string name)
    {
       Sound s = Array.Find(sfxSounds, x=> x.name == name);
       if(s == null)
       {
          Debug.Log("Sound Not Found");
       } else
       {
          sfxSource.PlayOneShot(s.clip);
       }
    }

}
