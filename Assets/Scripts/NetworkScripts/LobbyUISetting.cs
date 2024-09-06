using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUISetting : MonoBehaviour
{
    /* 온라인 모드 설정 방법
     * 1. 온라인 모드 버튼을 클릭해 마스터 서버에 접속한다 
     * 2. 로비창으로 이동한다 
     * 3. 초대하기 버튼과 방문하기 버튼 중 하나를 선택한다
     * 4-1. 초대하기를 클릭하면 방제와 인원수를 입력란이 뜨고 방을 생성, 입장한다.
     * 4-2. 방문하기 버튼을 클릭하면 방제 입력란이 뜨고 방제를 입력하고 해당 방으로 입장한다.
    */

    // 초대하기 방문하기 버튼
    public GameObject inviteButton;
    public GameObject visitButton;

    // 초대와 방문 창
    public GameObject inviteWindow;
    public GameObject visitWindow;

    void Start()
    {
        inviteWindow.SetActive(false);
        visitWindow.SetActive(false);
    }

    // 초대하기 버튼을 눌렀을 경우
    public void ClickInvite() 
    {
        inviteButton.SetActive(false);
        visitButton.SetActive(false);

        inviteWindow.SetActive(true);
    }
    // 방문하기 버튼을 눌렀을 경우
    public void ClickVisit() 
    {
        inviteButton.SetActive(false);
        visitButton.SetActive(false);

        visitWindow.SetActive(true);
    }

    // 닫기 버튼
    public void ClickClose() 
    {
        // 창을 닫는다.
        inviteWindow.SetActive(false);
        visitWindow.SetActive(false);

        // 입력한 내용을 지운다.

        // 초대 방문 버튼을 활성화한다.
        inviteButton.SetActive(true);
        visitButton.SetActive(true);
    }
}
