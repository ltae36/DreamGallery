using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpSpace : MonoBehaviour
{
    // 트리거에 진입하면 해당 장소로 이동한다.

    public GameObject moveButton;

    void Start()
    {
        moveButton.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        // 플레이어가 들어오면 씬 이동 버튼이 활성화된다.
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
