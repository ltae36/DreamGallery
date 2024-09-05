using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageMove : MonoBehaviour
{
    // 문을 클릭하면 문이 열리면서 다른 장소로 이동한다.

    public Animator animDoor;
    public Transform frontDoor;
    public GameObject player;
    Animator animPlayer;
    PlayerMove move;
    bool isEnter = false;

    void Start()
    {
        move = player.GetComponent<PlayerMove>();
        animPlayer = player.GetComponent<Animator>();
    }

    void Update()
    {
        if (isEnter)
        {
           // player.transform.position = Vector3.MoveTowards(player.transform.position, frontDoor.position, 5);
            
            player.transform.position += dir.normalized * 1 * Time.deltaTime;
        }
    }
    Vector3 dir;
    // 문을 클릭한다.(버튼 클릭)
    public void IntoDoor() 
    {
        dir = frontDoor.position - player.transform.position;
        // 문 열리는 애니메이션 재생
        animDoor.SetTrigger("Open");
        isEnter = true;
        // 플레이어가 문으로 걸어감
        // 플레이어가 문 앞에 서고 열린 문 안으로 걸어들어간다.
        //player.transform.position = frontDoor.position;
        //player.transform.rotation = Quaternion.identity;
        move.enabled = false;

        StartCoroutine(LoadSceneTime(5f));
        //player.GetComponent<PlayerMove>().EnterTheRoom();
        //animPlayer.SetTrigger("Enter");
        move.EnterTheRoom();
    }

    IEnumerator LoadSceneTime(float sec) 
    {
        yield return new WaitForSeconds(sec);
        // 씬이 이동됨
        SceneManager.LoadScene(1);
    }
}
