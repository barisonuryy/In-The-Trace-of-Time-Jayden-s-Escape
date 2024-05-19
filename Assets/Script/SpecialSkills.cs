using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSkills : MonoBehaviour
{
    // Start is called before the first frame update
    public Material[] material;
    public Shader Shader;
    public float lerpDuration = 2.0f;
    private float lerpTime = 0.0f;
    private bool lerping = false;
    [SerializeField] private float invisibleTime;
    public bool isInvisible;
    private float tempInvisibleTime;
    [SerializeField] private GameObject[] materials;

    private void Start()
    {
        materials[0].GetComponent<SkinnedMeshRenderer>().sharedMaterials[0].shader = material[0].shader;
        materials[1].GetComponent<SkinnedMeshRenderer>().sharedMaterials[0].shader = material[1].shader;
        tempInvisibleTime = invisibleTime;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E)&&invisibleTime>0)
        {
            materials[0].GetComponent<SkinnedMeshRenderer>().sharedMaterials[0].shader= Shader;
            materials[1].GetComponent<SkinnedMeshRenderer>().sharedMaterials[0].shader = Shader;
            isInvisible = true;
            

        }
       if (isInvisible)
        {
            invisibleTime -= Time.deltaTime;
        } 
        if (invisibleTime <= 0)
        {
            isInvisible = false;
            invisibleTime = tempInvisibleTime;
            materials[0].GetComponent<SkinnedMeshRenderer>().sharedMaterials[0].shader= material[0].shader;
            materials[1].GetComponent<SkinnedMeshRenderer>().sharedMaterials[0].shader = material[1].shader;
        }
        
        
    }
}
