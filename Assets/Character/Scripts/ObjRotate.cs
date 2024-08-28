using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotate : MonoBehaviour
{
    public float rotSpeed = 200;

    float rotX;
    float rotY;

    public bool useRotX;
    public bool useRotY;

    void Start()
    {
        
    }

    void Update()
    {
        // 1. ���콺�� �������� �޾ƿ���.
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // 2. ȸ�� ���� ����(����)
        if (useRotX) rotX += my * rotSpeed * Time.deltaTime;
        if (useRotY) rotY += mx * rotSpeed * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -80, 80);

        transform.localEulerAngles = new Vector3(-rotX, rotY, 0);

    }
}
