using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramePainting : MonoBehaviour
{
    public bool isClick;

    // �׸��� Ŭ���ϸ� �ش� �׸��� �ε��� ���� ������ �ȴ�.
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
