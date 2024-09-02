using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageMove : MonoBehaviour
{
    // 문을 클릭하면 문이 열리면서 다른 장소로 이동한다.

    public Animator animDoor;

    public GameObject player;
    Animator animPlayer;


    void Start()
    {
        
    }

    void Update()
    {

    }

    // 문을 클릭한다.(버튼 클릭)
    void IntoDoor() 
    {
        // 문 열리는 애니메이션 재생
        animDoor.SetTrigger("Open");

        // 플레이어가 문으로 걸어감


        // 씬이 이동됨
        SceneManager.LoadScene("");
    }
}
