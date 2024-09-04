using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotate : MonoBehaviour
{
    public float rotSpeed = 400;

    float rotX;
    float rotY;

    public bool useRotX;
    public bool useRotY;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            // 1. 마우스의 움직임을 받아오자.
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            // 2. 회전 값을 변경(누적)
            if (useRotX) rotX += my * rotSpeed * Time.deltaTime;
            if (useRotY) rotY += mx * rotSpeed * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -80, 80);

            transform.localEulerAngles = new Vector3(-rotX, rotY, 0);
        }

    }
}
