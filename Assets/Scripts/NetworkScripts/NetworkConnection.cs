using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class NetworkConnection : MonoBehaviourPunCallbacks
{
    // 로그인을 하면 온라인 마스터서버로 접속하고 로비로 이동함
    // 방은 자동으로 생성되고 보이게 할 지, 초대 할 지를 플레이어가 컨트롤
    //public GameObject mychar;
    //public TMP_Text roomNameMark;

    void Start()
    {
        //// 멀티플레이 상태라면
        //if (PhotonNetwork.IsConnected) 
        //{
        //    mychar.SetActive(false); 
        //    roomNameMark.text = PhotonNetwork.CurrentRoom.Name;
        //}
    }

    public void OnClickConnect() 
    {
        // 로그인 버튼을 누르면 마스터 서버에 접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버에 접속이 되면 호출되는 함수
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();


        //// 아이디 설정
        //int charID = Random.Range(100, 999);
        //PhotonNetwork.NickName = "꿈빛화가" + charID;

        // 로비에 참가
        PhotonNetwork.JoinLobby();

    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        // 갤러리 씬으로 이동
        PhotonNetwork.LoadLevel("ConnectScene");
        print("로비 진입 성공");
    }

    public void MoveSetting() 
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}
