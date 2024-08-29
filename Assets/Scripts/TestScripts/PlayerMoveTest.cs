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
        // 캐릭터를 상하좌우 움직인다.
        // 1 .키보드 WASD 키 입력을 받자
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. 방향을 정하자.
        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * v;
        Vector3 dir = dirH + dirV;

        dir.Normalize();

        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}
