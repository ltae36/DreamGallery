using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    public float moveSpeed = 8f;

    void Start()
    {
        
    }

    void Update()
    {
        // ĳ���͸� �����¿� �����δ�.
        // 1 .Ű���� WASD Ű �Է��� ����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. ������ ������.
        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * v;
        Vector3 dir = dirH + dirV;

        dir.Normalize();

        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}
