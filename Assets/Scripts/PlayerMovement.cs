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
    
    [Header("Aim Info")]
    private Vector3 lookingDirection;
    [SerializeField] private LayerMask aimLayer;
    [SerializeField] private Transform aim;

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
        AimTowardsMouse();
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

    private void AimTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if (Physics.Raycast(ray, out var hitIfo, Mathf.Infinity, aimLayer))
        {
            lookingDirection = hitIfo.point - transform.position;
            lookingDirection.y = 0;
            lookingDirection.Normalize();
            
            transform.forward = lookingDirection;
            
            aim.position = new Vector3(hitIfo.point.x, transform.position.y + 1, hitIfo.point.z);
        }
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
        
        controls.Charactor.Aim.performed += ctx=>aimInput = ctx.ReadValue<Vector2>();
        controls.Charactor.Aim.canceled += ctx=>aimInput = Vector2.zero;
        
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
