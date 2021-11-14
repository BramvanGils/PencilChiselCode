using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArm : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private Player player;

    private bool active;
    private bool inserting;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Activate()
    {
        active = true;
        animator.SetTrigger("Activate");
    }

    public void Insert()
    {
        if (active)
        {
            player.OnInsertionStart();
            inserting = true;
            animator.SetTrigger("Insert");
        }
    }

    public void OnInsertFinish()
    {
        inserting = false;
        player.OnInsertionEnd();
    }
}
