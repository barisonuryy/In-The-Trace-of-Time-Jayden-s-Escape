using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSkills : MonoBehaviour
{
    // Start is called before the first frame update
    public Material material;
    public float lerpDuration = 2.0f;
    private float lerpTime = 0.0f;
    private bool lerping = false;
    [SerializeField] private GameObject[] materials;

    private void Start()
    {
        materials[0].GetComponent<SkinnedMeshRenderer>().sharedMaterials[0] =
            materials[0].GetComponent<SkinnedMeshRenderer>().materials[0];
        materials[1].GetComponent<SkinnedMeshRenderer>().sharedMaterials[0] =
            materials[1].GetComponent<SkinnedMeshRenderer>().materials[0];
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
           // materials[0].GetComponent<SkinnedMeshRenderer>().sharedMaterials[0].
           // materials[1].GetComponent<SkinnedMeshRenderer>().sharedMaterials[0]
              

        }
       
        
    }
}
