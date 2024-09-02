using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpSpace : MonoBehaviour
{
    // Ʈ���ſ� �����ϸ� �ش� ��ҷ� �̵��Ѵ�.
    public GameObject moveButton;

    Animator anim;

    void Start()
    {
        moveButton.SetActive(false);

        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        // �÷��̾ ������ �� �̵� ��ư�� Ȱ��ȭ�ȴ�.
        if(other.gameObject.tag == "Player") 
        {
            anim.SetTrigger("Open");
            moveButton.SetActive(true);
        }
    }


    public void MoveMent() 
    {
        SceneManager.LoadScene(1);
    }
}
