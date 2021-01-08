﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public Animator animator;

    public float speed = 5f;

    public float gravity = 19.62f;

    public float jumpHeight = 6f;

    private Vector3 velocity;

    public float originalHeight;

    public float crouchHeight;

    private void Start()
    {
        originalHeight = controller.height;
    }

    private void Update()
    {
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
            velocity.y = jumpHeight;

        if (Input.GetKeyDown(KeyCode.LeftControl))
            controller.height = crouchHeight;

        if (Input.GetKeyUp(KeyCode.LeftControl))
            controller.height = originalHeight;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            speed *= 2;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            speed /= 2;
    }

    void FixedUpdate()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        var direction = transform.right * x + transform.forward * z;

        direction.Normalize();

        controller.Move(direction * speed * Time.deltaTime);

        velocity.y -= gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            velocity.y = -2f;
        }
    }
}
