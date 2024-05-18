using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CMov : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    [Header("Ground Check Settings")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;
    private Vector2 moveInputN;
    private Vector3 velocityJump;
    private bool isMove;
    private bool isJump;
    private float previousMoveAmount=0f;
    bool isGrounded;
    private float moveAmount;
    float ySpeed;

    Quaternion targetRotation;

    CameraController cameraController;
    Animator animator;
    CharacterController characterController;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    public void OnMove(InputAction.CallbackContext value)
    {
        moveInputN = value.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (isGrounded)
        {
            velocityJump.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJump = true;
        }
    }
    private void Update()
    {
 
      moveAmount = Mathf.Clamp01(Mathf.Abs(moveInputN.x) + Mathf.Abs(moveInputN.y));


        var moveInput = (new Vector3(moveInputN.x, 0, moveInputN.y)).normalized;
        var moveDir = cameraController.PlanarRotation * moveInput;

        GroundCheck();
        //Debug.Log("IsGrounded = " + ÝsGrounded);
        if (isGrounded && velocityJump.y < 0)
        {
            velocityJump.y = -2f;
           
        }
        velocityJump.y += gravity * Time.deltaTime;
        var velocity = moveDir * moveSpeed;
        velocity.y = velocityJump.y;
        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
       
            animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);
            

    }
    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }

    private void OnAnimatorMove()
    {
        animator.SetBool("canJump",isJump);
        animator.SetBool("isGround",isGrounded);
        if (isJump)
        {
            isJump = false;
        }

        if (moveAmount > 0.2)
        {
            animator.SetFloat("moveAmount", 0, 0.2f, Time.deltaTime);
        }
    }
}
