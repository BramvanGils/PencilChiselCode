using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    private CharacterController controller;
    private AudioSource audioSource;
    private Animator animator;

    [SerializeField] private PostProcessingBehaviour processingBehaviour;
    [SerializeField] private Transform meshHandle;

    private bool facingLeft = true;
    private bool paralyzed;
    public float movementSpeed = 1f;
    

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!paralyzed) Move();
        else animator.SetBool("Walking", false);
    }

    private void Move()
    {
        //Find Forces
        Vector3 verticalMovement = Vector3.forward * Input.GetAxisRaw("Vertical");
        Vector3 horizontalMovement = Vector3.right * Input.GetAxisRaw("Horizontal");
        Vector3 gravity = Vector3.down * 2f;

        //Calculate Motion
        Vector3 result = verticalMovement + horizontalMovement;
        if (result.magnitude > 1f)
            result = result.normalized;
        animator.SetBool("Walking", result.magnitude > 0.1f);

        result += gravity;
        result *= movementSpeed * Time.deltaTime;
        controller.Move(result);

        //Resolve Animation
        if (facingLeft && result.x > 0)
        {
            facingLeft = false;
            processingBehaviour.SlideVignetteFocus(facingLeft);
            meshHandle.localScale = new Vector3(-1, 1, 1);
        }
        if (!facingLeft && result.x < 0)
        {
            facingLeft = true;
            processingBehaviour.SlideVignetteFocus(facingLeft);
            meshHandle.localScale = new Vector3(1, 1, 1);
        }
    }

    public void OnInsertionStart()
    {
        paralyzed = true;
        animator.SetTrigger("Insert");
        audioSource.Play();
        animator.SetTrigger("TriggerWrithe");
    }

    public void OnInsertionEnd()
    {
        paralyzed = false;
        processingBehaviour.IncreaseIntensity();
    }
}
