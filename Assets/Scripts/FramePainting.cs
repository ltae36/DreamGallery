using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramePainting : MonoBehaviour
{
    public bool isClick;

    MeshRenderer mr;
    BottonManager bm;

    void Start()
    {
        isClick = false;
        mr = GetComponent<MeshRenderer>();
    }

    void Update()
    {

    }

}
