using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramePainting : MonoBehaviour
{
    public bool isClick;

    // 그림을 클릭하면 해당 그림의 인덱스 값이 저장이 된다.
    public string picIndex;

    Material mat;
    Texture tex;

    void Start()
    {
        isClick = false;
        mat = GetComponent<Material>();
        tex = GetComponent<Texture>();
    }

    void Update()
    {

    }

}
