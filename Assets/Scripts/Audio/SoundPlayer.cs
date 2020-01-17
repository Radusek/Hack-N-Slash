﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] audioClips;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int clipIndex)
    {
        audioSource.PlayOneShot(audioClips[clipIndex]);
    }
}
