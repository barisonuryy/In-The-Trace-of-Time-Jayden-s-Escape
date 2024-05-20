using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSystem : MonoBehaviour
{
    
    [SerializeField] private GameObject weapon;
    private bool isShoot;
    private bool isAim;
    [SerializeField] private Transform spawnBulletPosition;
    private Animator anim;
    [SerializeField] private GameObject pfBulletProjectile;
    private CMov characterMovement;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private float sensivity,FOV,newDistance;
    [SerializeField] private Camera camera;
    [SerializeField] LayerMask aimColliderLayerMask;
    private float realFOV,realSens,realMovSpeed,realDistance;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        characterMovement = GetComponent<CMov>();
        realFOV = camera.fieldOfView;
        realSens = sensivity;
        realDistance = _cameraController.distance;
        realMovSpeed = characterMovement.moveSpeed;
        
  
    }

    void checkWeaponState()
    {
       
    }

    public void OnShoot(InputAction.CallbackContext val)

        {
            if (val.performed)
            {
                isShoot = true;
                anim.SetTrigger("Shoot");
            }

            if (val.action.WasReleasedThisFrame())
            {
                anim.ResetTrigger("Shoot");
            }
            
        }

    public void OnAim(InputAction.CallbackContext val)
    {
        if(val.performed)
        isAim = !isAim;
    }

    // Update is called once per frame
    void Update()
    {

        
        Vector3 mouseWorldPosition = Vector3.zero;
        
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = camera.ScreenPointToRay(screenCenterPoint);
        
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 99999f, aimColliderLayerMask)) {
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
           
        }

        if (isAim) {
           setOnAim();
            anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 1f, Time.deltaTime * 13f));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        } else {
            setOffAim();
            anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 0f, Time.deltaTime * 13f));
        }


        if (isShoot) {
            /*
            // Hit Scan Shoot
            if (hitTransform != null) {
                // Hit something
                if (hitTransform.GetComponent<BulletTarget>() != null) {
                    // Hit target
                    Instantiate(vfxHitGreen, mouseWorldPosition, Quaternion.identity);
                } else {
                    // Hit something else
                    Instantiate(vfxHitRed, mouseWorldPosition, Quaternion.identity);
                }
            }
            //*/
            //*
            // Projectile Shoot
            anim.SetLayerWeight(1, Mathf.Lerp(anim.GetLayerWeight(1), 1f, Time.deltaTime * 13f));
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            //*/
            isShoot = false;
        }

    }
    void setOnAim()
    {
        camera.fieldOfView = FOV;
        _cameraController.rotationSpeed = sensivity;
        characterMovement.moveSpeed = 0;
        _cameraController.distance = newDistance;

    }

    void setOffAim()
    {
        camera.fieldOfView = realFOV;
        _cameraController.rotationSpeed = realSens;
        characterMovement.moveSpeed = realMovSpeed;
        _cameraController.distance = realDistance;

    }

    private void OnAnimatorMove()
    {
        anim.SetBool("Aiming",isAim);
        
    }
}
