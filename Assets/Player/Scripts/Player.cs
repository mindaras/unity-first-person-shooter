using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;

    [SerializeField]
    private float _jumpHeight = 5f;

    [SerializeField]
    private float _gravity = 0.5f;

    private float _velocity;

    private CharacterController _controller;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _velocity = _jumpHeight;
            }
        }
    }

    void FixedUpdate()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        var direction = new Vector3(horizontalInput, _velocity, verticalInput);
        var velocity = direction * _speed;

        if (!_controller.isGrounded)
        {
            _velocity -= _gravity;
        }

        _controller.Move(velocity * Time.deltaTime);
    }
}
