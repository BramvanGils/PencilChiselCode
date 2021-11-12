using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    CharacterController controller;

    public float movementSpeed = .001f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 verticalMovement = Vector3.forward * Input.GetAxisRaw("Vertical");
        Vector3 horizontalMovement = Vector3.right * Input.GetAxisRaw("Horizontal");
        Vector3 result = verticalMovement + horizontalMovement;
        if (result.magnitude > 1f)
            result = result.normalized;
        result *= movementSpeed * Time.deltaTime;
        controller.Move(result);
    }
}
