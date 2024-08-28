using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // ĳ���� ��Ʈ�ѷ�
    CharacterController cc;
    // �̵� �ӷ�
    public float moveSpeed = 5.0f;
    // �߷�
    float gravity = -9.8f;
    // y �ӷ�
    float yVelocity;
    // ���� �ʱ� �ӷ�
    public float jumpPower = 3;

    public GameObject cam;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * v;
        Vector3 dir = dirH + dirV;
        dir.Normalize();
        
        if(cc.isGrounded)
        {
            yVelocity = 0;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            yVelocity = jumpPower;
        }

        yVelocity += gravity * Time.deltaTime;

        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);
    }
}
