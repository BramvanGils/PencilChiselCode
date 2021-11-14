using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{
    private AudioSource audioSource;
    
    [SerializeField] private AudioClip[] footsteps;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnFootstep()
    {
        audioSource.clip = footsteps[Random.Range(0, footsteps.Length - 1)];
        audioSource.Play();
    }
}
