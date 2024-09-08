using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionTest : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("마스터 서버에 접속");

        //로비 접속
        JoinLobby();
    }

    private void JoinLobby()
    {
        //닉네임 설정
        PhotonNetwork.NickName = "user" + Random.Range(1, 1000);
        //기본 Lobby 입장
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 입장 완료");

        JoinOrCreateRoom();
    }

    private void JoinOrCreateRoom()
    {
        //방 생성 옵션
        RoomOptions roomOption = new RoomOptions();
        //방에 들어 올 수 있는 최대 인원 설정
        roomOption.MaxPlayers = 20;
        //로비에 방을 보이게 할 것인가
        roomOption.IsVisible = true;
        //방에 참여를 할 수 있니?
        roomOption.IsOpen = true;

        //Room 참여 or 생성
        PhotonNetwork.JoinOrCreateRoom("paint_multy_room1234", roomOption, TypedLobby.Default);
    }

    //방 생성 성공 했을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 입장 완료");
        //멀티플레이 컨텐츠를 즐길 수 있는 상태
        //메인으로 이동
        PhotonNetwork.LoadLevel("PaintingScene");
    }
}
