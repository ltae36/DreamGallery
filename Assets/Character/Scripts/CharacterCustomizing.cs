using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizing : MonoBehaviour
{
    public Material[] mat = new Material[2];
    public static SkinnedMeshRenderer[] all;
    int i = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            ChangeCharacterMat();
        }
    }


    public void ChangeCharacterMat()
    {
        i = ++i % 2;

        // Change Material
        all = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer material in all)
        {
            material.material = mat[i];

        }
        print("1111111111111");
    }
}
