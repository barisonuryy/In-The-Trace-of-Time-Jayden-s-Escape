using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroController : MonoBehaviour
{
    Vector3 StartPos_Hero;
    public Transform transformHero;
    private Animator _animator;
    private void Awake()
    {
        StartPos_Hero = transformHero.position;

    }
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        MoveRightLeft();
        MoveUpDown();
        Invisibility();
    }
    private void MoveRightLeft()
    {
        Vector3 vectorLeft = Vector3.zero;
        vectorLeft.x = Input.GetAxis("Horizontal");
        Vector3 v = new Vector3(vectorLeft.x, 0.0f, 0.0f)*Time.deltaTime*5.0f;
        transformHero.Translate(v, Space.Self);
    }
    private void MoveUpDown()
    {
        Vector3 vectorUp = Vector3.zero;
        vectorUp.z = Input.GetAxis("Vertical");
        Vector3 v = new Vector3(0.0f, 0.0f, vectorUp.z) * Time.deltaTime * 5.0f;
        transformHero.Translate(v, Space.Self);
    }

    private void Invisibility()
    {
        bool invisible = Input.GetKey(KeyCode.F);
        if (invisible)
        {
            _animator.SetBool("isInvisible", true);
        }
        else
        {
            _animator.SetBool("isInvisible", false);
        }
    }
}
