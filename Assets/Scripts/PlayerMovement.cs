using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;
    private CharacterController characterController;
    private Animator animator;
    
    private Vector2 moveInput;
    private Vector2 aimInput;
    private bool isRunning;

    [Header("Movement Info")]
    private float speed;
    [SerializeField] private float walkSpeed;
    [SerializeField]private float runSpeed;
    private Vector3 moveDirection;
    private float verticleVelocity;

    private void Start()
    {
        player = GetComponent<Player>();
        
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        
        speed = walkSpeed;
        
        AssingInputEvents();
    }

    private void Update()
    {
        ApplyMovement();
        ApplyRotation();
        AnimatorControllers();
    }

    private void AnimatorControllers()
    {
        float xVelocity = Vector3.Dot(moveDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(moveDirection.normalized, transform.forward);
        
        animator.SetFloat("xVelocity", xVelocity,.1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity,.1f, Time.deltaTime);
        
        bool playRunAnimation = moveDirection.sqrMagnitude > 0 && isRunning;
        animator.SetBool("isRunning", playRunAnimation);
    }

    private void ApplyRotation()
    {
            var lookingDirection = player.aim.GetMousePosition() - transform.position;
            lookingDirection.y = 0;
            lookingDirection.Normalize();
            
            transform.forward = lookingDirection;
    }

    private void ApplyMovement()
    {
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();
        if (moveDirection.sqrMagnitude > 0)
        {
            characterController.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            verticleVelocity-= 9.81f * Time.deltaTime;
            moveDirection.y = verticleVelocity;
        }
        else
        {
            verticleVelocity = -0.5f;
        }
    }

    private void AssingInputEvents()
    {
        controls = player.controls;
        
        controls.Charactor.Movement.performed += ctx=>moveInput = ctx.ReadValue<Vector2>();
        controls.Charactor.Movement.canceled += ctx=>moveInput = Vector2.zero;
        
        controls.Charactor.Run.performed+=ctx=>
        {
            speed = runSpeed;
            isRunning = true;
        };
        controls.Charactor.Run.canceled+=ctx=>
        {
            speed = walkSpeed;
            isRunning = false;
        };
    }
}
