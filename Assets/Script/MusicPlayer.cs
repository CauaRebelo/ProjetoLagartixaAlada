using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    public AudioSource src;
    public AudioClip musica;
    public bool precisaGuardar;
    public float storeTime;
    public float volMusic = 1f;

    void Awake()
    {
        src = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>()._audioSource;
        if (precisaGuardar) 
        {
            storeTime = src.time;
            src.Stop();
            src.clip = musica;
            src.Play();
            src.volume = volMusic;
            src.time = storeTime;
        }
        else
        {
            src.Stop();
            src.clip = musica;
            src.Play();
            src.volume = volMusic;
        }
    }

}
