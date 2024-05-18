using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageCheck : MonoBehaviour
{
    private Animator anim;
    public bool isAttacked;
    [SerializeField] private Actor _actor; 
    [SerializeField] private int attackDamage;
    // Start is called before the first frame update

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    private void Update()
    {
   
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacked && other.gameObject.CompareTag("Enemy"))
        {
            
            _actor.TakeDamage(attackDamage);
        }
    }

    private void OnAnimatorMove()
    {
        anim.SetBool("canAttack",isAttacked);
    }
}
