using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;
using UnityEditor.XR;

public class NetworkConnection : MonoBehaviourPunCallbacks
{
    // 갤러리에서 온라인 모드를 켜면 방이 열리고 초대할 수 있음
    // 방은 자동으로 생성되고 보이게 할 지, 초대 할 지를 플레이어가 컨트롤

    /* 1. 마스터 서버에 입장한다 
     * 2. 로비에 입장한다 
     * 3. 방을 만들고 방 이름과 비밀번호를 설정한다
     * 4. 방에 입장을 시도한다.
     * 5. 실패했다면 로비로 돌아간다 */
    public string enteredPassword = null; // 사용자가 입력한 비밀번호를 저장하는 변수
    public string roomName;    

    void Start()
    {
        // Photon 환경을 기반으로 접속을 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버에 접속이 되면 호출되는 함수
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("마스터 서버에 접속");

        // 로비 접속
        JoinLobby();
    }

    public void JoinLobby()
    {
        // 닉네임 설정
        PhotonNetwork.NickName = "꿈화가";
        // 기본 Lobby 입장
        PhotonNetwork.JoinLobby();
    }

    // 로비에 참여를 성공하면 호출되는 함수
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 입장 완료");

    }

    // Room 을 참여하자. 만약에 해당 Room 이 없으면 Room 만들겠다.
    public void CreateRoom()
    {
        // 방 생성 옵션
        RoomOptions roomOpt = new RoomOptions();

        roomOpt.MaxPlayers = 20; // 방에 들어올 수 있는 최대인원
        roomOpt.IsVisible = false; // 로비에 방을 보이게 할 지 
        roomOpt.IsOpen = true; // 방에 참여할 수 있는지
        roomOpt.CustomRoomProperties = new Hashtable()
        {
            // 비밀번호 설정
            {"Password", enteredPassword}
        };
        // 로비에서 비밀번호를 확인할 수 있도록 설정
        roomOpt.CustomRoomPropertiesForLobby = new string[] { "Password" };

        // Room 참여 or 생성
        PhotonNetwork.CreateRoom(roomName, roomOpt, TypedLobby.Default);
    }

    // 방에 입장할 때 비밀번호를 입력하게 하는 방법
    public void JoinRoomWithPassword()
    {
        PhotonNetwork.JoinRoom(roomName);
        // 암호를 입력받아서 암호가 맞다면 방에 입장한다. 아니라면 다시 입력한다.
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Password"))
        {
            string roomPassword = PhotonNetwork.CurrentRoom.CustomProperties["Password"].ToString();
            if (enteredPassword != roomPassword)
            {
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                print("환영합니다!");
            }
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
    }

    // 방 입장 성공했을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        print("입장 대기 중!");
        base.OnJoinedRoom();
        
    }

    // 방 생성이 성공했을 때 호출되는 함수
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료");
    }
}
