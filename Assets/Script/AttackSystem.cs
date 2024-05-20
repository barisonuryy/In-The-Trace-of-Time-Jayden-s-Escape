using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackSystem : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    AudioSource audioSource;

    [SerializeField] private Vector3 offset;

    Vector3 _PlayerVelocity;
    

    [Header("Camera")]
    public Transform sword;

    public Camera cam;
    public float sensitivity;

    float xRotation = 0f;

    void Awake()
    { 
       
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

      
        
 

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



   

  

    // ---------- //
    // ANIMATIONS //
    // ---------- //


    public const string ATTACK1 = "Attack 1";
    public const string ATTACK2 = "Attack 2";

    string currentAnimationState;

    public void ChangeAnimationState(string newState) 
    {
        // STOP THE SAME ANIMATION FROM INTERRUPTING WITH ITSELF //
        if (currentAnimationState == newState) return;

        // PLAY THE ANIMATION //
        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    }



    // ------------------- //
    // ATTACKING BEHAVIOUR //
    // ------------------- //

    [Header("Attacking")]
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;

    public GameObject hitEffect;
    public AudioClip swordSwing;
    public AudioClip hitSound;

    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;

    public void Attack(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            if(!readyToAttack || attacking) return;

            readyToAttack = false;
            attacking = true;

            Invoke(nameof(ResetAttack), attackSpeed);
            Invoke(nameof(AttackRaycast),attackDelay);
        

//        audioSource.pitch = Random.Range(0.9f, 1.1f);
            //  audioSource.PlayOneShot(swordSwing);

            if(attackCount == 0)
            {
                ChangeAnimationState(ATTACK1);
                attackCount++;
            }
            else
            {
                ChangeAnimationState(ATTACK2);
                attackCount = 0;
            }
        }
       
        
         }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }
    void AttackRaycast()
    {
        Vector3 direction = sword.TransformDirection(Vector3.up);
        if(Physics.Raycast(sword.position+direction, direction, out RaycastHit hit, attackDistance, attackLayer))
        { 
           // HitTarget(hit.point);
               Debug.Log("Attack Başarılı");
            if(hit.transform.TryGetComponent<Actor>(out Actor T))
            { T.TakeDamage(attackDamage); }
        } 
    }

    void HitTarget(Vector3 pos)
    {
       // audioSource.pitch = 1;
       // audioSource.PlayOneShot(hitSound);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color=Color.magenta;
        Vector3 direction = sword.TransformDirection(Vector3.up);
        
        Gizmos.DrawRay(sword.position+direction, direction);
    }
}
