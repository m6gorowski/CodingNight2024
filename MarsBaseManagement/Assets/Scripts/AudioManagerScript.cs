using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerScript : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource musicSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _backgroundMusic;

    private void Start()
    {
        musicSource.clip = _backgroundMusic;
        musicSource.Play();
    }
}
