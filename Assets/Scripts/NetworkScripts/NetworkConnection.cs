using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkConnection : MonoBehaviourPunCallbacks
{
    // 로그인을 하면 온라인 마스터서버로 접속하고 로비로 이동함
    // 방은 자동으로 생성되고 보이게 할 지, 초대 할 지를 플레이어가 컨트롤
    //public GameObject mychar;

    public TMP_InputField userID;
    bool checkLeftRoom;
    string roomName;

    void Start()
    {
        checkLeftRoom = false;
        //// 멀티플레이 상태라면
        //if (PhotonNetwork.IsConnected) 
        //{
        //    mychar.SetActive(false); 
        //    roomNameMark.text = PhotonNetwork.CurrentRoom.Name;
        //}

        // 인풋필드의 내용이 변경되었을 때 호출되는 함수
        userID.onValueChanged.AddListener(OnValueChangedRoomID);
    }



    // 로그인 버튼을 눌렀을 때
    public void OnClickConnect()
    {
        // 로그인 버튼을 누르면 마스터 서버에 접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버에 접속이 되면 호출되는 함수
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();


        // 아이디 설정
        int charID = Random.Range(100, 999);
        PhotonNetwork.NickName = "꿈빛화가" + charID;

        // 로비에 참가
        PhotonNetwork.JoinLobby();

    }

    // 로비에 접속이 되면 호출되는 함수
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 진입 성공");
        // 로비에 들어왔을 때 방을 나갔다 들어온 상태라면 single 모드로 넘어가고 아니라면 로비씬으로 넘어감
        if (!checkLeftRoom)
        {
            // 로딩 화면으로 이동
            PhotonNetwork.LoadLevel("LobbyScene");
            // 자동으로 방 생성
            CreateNewRoom();
        }
        else
        {
            PhotonNetwork.LoadLevel("ConnectSceneSingle");
        }


    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        checkLeftRoom = true;
    }

    // 입력에 따른 버튼 활성화
    void OnValueChangedRoomID(string roomName)
    {
        // 아이디가 입력되었을 때
        userID.interactable = roomName.Length > 0;
    }

    #region 방 생성
    public void CreateNewRoom()
    {
        // 방 옵션 설정
        RoomOptions option = new RoomOptions();
        // 공개 여부 설정
        option.IsVisible = true;
        // 최대 인원 설정
        option.MaxPlayers = 20;
        // 방 이름 설정
        roomName = userID.text;
        // 방을 생성
        PhotonNetwork.CreateRoom(roomName, option);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("방 생성 실패 : " + message);
    }
    #endregion

    #region 방 입장
    public void JoinRoom()
    {
        // 방문하기 버튼을 누르면 방을 떠난다.
        PhotonNetwork.LeaveRoom();
    }

    public void VisitRoom()
    {
        // 방 입장 요청
        PhotonNetwork.JoinRoom(userID.text);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("어서오세요!");
        PhotonNetwork.LoadLevel("ConnectScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("방을 찾을 수 없습니다!" + message);
    }
    #endregion
}
