using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    private Animator anim;
    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isMove;
    private bool isJump;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        moveInput = value.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJump = true;
        }
    }

    private void Update()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
           
        }

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
  
        
        characterController.Move(move * Time.deltaTime * moveSpeed);
        isMove = move != Vector3.zero;

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void OnAnimatorMove()
    {
        anim.SetBool("canJump",isJump);
        anim.SetBool("canMove",isMove);
        anim.SetBool("isGround",isGrounded);
        if (isJump)
        {
            isJump = false;
        }
    }
}
