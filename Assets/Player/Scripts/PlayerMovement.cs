using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 5f;

    public float gravity = 19.62f;

    public float jumpHeight = 6f;

    private Vector3 velocity;

    private void Update()
    {
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpHeight;
        }
    }

    void FixedUpdate()
    {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");
        var direction = transform.right * x + transform.forward * z;
        controller.Move(direction * speed * Time.deltaTime);
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            velocity.y = -2f;
        }
    }
}
