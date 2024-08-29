using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpSpace : MonoBehaviour
{
    // Ʈ���ſ� �����ϸ� �ش� ��ҷ� �̵��Ѵ�.

    public GameObject moveButton;

    void Start()
    {
        moveButton.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        // �÷��̾ ������ �� �̵� ��ư�� Ȱ��ȭ�ȴ�.
        if(other.gameObject.tag == "Player") 
        {
            moveButton.SetActive(true);
        }
    }


    public void MoveMent() 
    {
        SceneManager.LoadScene(1);
    }
}
