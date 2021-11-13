using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class Grimace : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] whispers;
    [SerializeField] private AudioClip gasp;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            AudioClip clip = whispers[Random.Range(0, whispers.Length - 1)];
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
