using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CMov : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    [SerializeField] private float runSpeed,crouchSpeed,height,wide;
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
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;
    [SerializeField] private Camera cam;
    private float currentDashTime;
    private bool isDashing;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    private bool isRunning;
    private float tempRunSpeed,tempCrouchSpeed,tempHeight,tempWide;
    Quaternion targetRotation;
    [SerializeField] private float runCoolDown,regenateTime;
    [SerializeField] CameraController cameraController;
    Animator animator;
    private bool isCrouched;
    CharacterController characterController;

    private void Awake()
    {
        tempRunSpeed = runSpeed;
        tempCrouchSpeed = crouchSpeed;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        tempWide = characterController.radius;
        tempHeight = characterController.height;
        runSpeed = 1f;
        crouchSpeed = 1f;
    }
    public void OnMove(InputAction.CallbackContext value)
    {
        if(!isDashing&&isGrounded)
        moveInputN = value.ReadValue<Vector2>();
        if (isDashing)
        {
            moveInputN=Vector2.zero;
        }

    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (isGrounded)
        {
            velocityJump.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJump = true;
        }
    }

    public void OnCrouch(InputAction.CallbackContext val)
    {
        if (val.performed)
        {
            characterController.height = height;
            characterController.radius = wide;
            isCrouched = true;
            crouchSpeed = tempCrouchSpeed;
        }

        if(val.action.WasReleasedThisFrame())
        {
            characterController.height = tempHeight;
            characterController.radius = tempWide;
            isCrouched = false;
            crouchSpeed = 1f;
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && !isDashing)
        {
            isDashing = true;
            currentDashTime = dashTime;
        }
    }
    public void HandleDash()
    {
        if (isDashing)
        {
            if (currentDashTime > 0)
            {
                Debug.Log("Dash Başarılı");
                Vector3 dashDirection = moveDirection != Vector3.zero ? moveDirection : transform.forward;
                characterController.Move(dashDirection * dashSpeed*Time.deltaTime);
                isMove = false;
                currentDashTime -= Time.deltaTime;
            }
            else
            {
               
               
                isDashing = false;
            } 
        }
         
        
    }
    private void Update()
    {
     HandleDash();
      moveAmount = Mathf.Clamp01(Mathf.Abs(moveInputN.x) + Mathf.Abs(moveInputN.y));
      if (isRunning && regenateTime>0)
      {
          
          runSpeed -= Time.deltaTime / 2;
          regenateTime -= Time.deltaTime/2;

      }
      else
      {
          isRunning = false;
          regenateTime += Time.deltaTime/4;
          runSpeed = 1f;
      }

  
       
      

        var moveInput = (new Vector3(moveInputN.x, 0, moveInputN.y)).normalized;
        var moveDir = cameraController.PlanarRotation * moveInput;

        GroundCheck();
        //Debug.Log("IsGrounded = " + ÝsGrounded);
        if (isGrounded && velocityJump.y < 0)
        {
            velocityJump.y = -2f;
           
        }
        velocityJump.y += gravity * Time.deltaTime;
        var velocity = moveDir * moveSpeed*runSpeed*crouchSpeed;
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
        animator.SetBool("isRunning",isRunning);
        animator.SetBool("canCrouch",isCrouched);
        if (isJump)
        {
            isJump = false;
        }

     }

    public void OnRunning(InputAction.CallbackContext val)
    {
        if (val.performed)
        {
            isRunning=true;
            runSpeed = tempRunSpeed;
        }

        if (val.action.WasReleasedThisFrame())
        {
            isRunning = false;
        }  
    }
    
    }

