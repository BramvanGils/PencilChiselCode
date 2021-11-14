using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Grimace : MonoBehaviour
{
    private AudioSource audioSource;
    private Animator animator;

    [SerializeField] private AudioClip[] whispers;
    [SerializeField] private AudioClip gasp;
    [SerializeField] private AudioClip attack;

    private bool whispering = true;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!audioSource.isPlaying && whispering)
        {
            AudioClip clip = whispers[Random.Range(0, whispers.Length - 1)];
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void Gasp() 
    {
        animator.SetTrigger("Gasp");
        whispering = false;
        audioSource.Stop();
        audioSource.clip = gasp;
        audioSource.Play();
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        audioSource.Stop();
        audioSource.clip = attack;
        audioSource.Play();
        transform.localScale = new Vector3(-1, 1, 1);
    }
}
