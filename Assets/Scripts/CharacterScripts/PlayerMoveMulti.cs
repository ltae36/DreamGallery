using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerMoveMulti : MonoBehaviourPun
{
    // 캐릭터 컨트롤러
    public CharacterController cc;
    // 이동 속력
    public float moveSpeed = 5.0f;
    // 중력
    float gravity = -9.8f;
    // y 속력
    float yVelocity;
    // 점프 초기 속력
    public float jumpPower = 3;

    public GameObject cam;

    Animator anim;

    // 닉네임
    public TMP_Text nick;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        nick.text = photonView.Owner.NickName;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector3 dirH = transform.right * h;
            Vector3 dirV = transform.forward * v;
            Vector3 dir = dirH + dirV;
            dir.Normalize();

            if (dir != Vector3.zero)
            {
                anim.SetBool("IsWalking", true);
            }
            else
            {
                anim.SetBool("IsWalking", false);
            }


            if (cc.isGrounded)
            {
                yVelocity = 0;
                anim.SetBool("IsJumping", false);
            }

            if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded)
            {
                yVelocity = jumpPower;
                anim.SetBool("IsJumping", true);
            }

            yVelocity += gravity * Time.deltaTime;

            dir.y = yVelocity;

            cc.Move(dir * moveSpeed * Time.deltaTime);

            // anim을 이용해서 h, v 값을 전달
            anim.SetFloat("DirH", h);
            anim.SetFloat("DirV", v);
        }

    }

    public void EnterTheRoom()
    {
        anim.SetTrigger("Enter");

    }
}
