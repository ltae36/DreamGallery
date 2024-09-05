using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEditor.XR;

public class NetworkConnection : MonoBehaviour
{
    // 갤러리에서 온라인 모드를 켜면 방이 열리고 초대할 수 있음
    // 방은 자동으로 생성되고 보이게 할 지, 초대 할 지를 플레이어가 컨트롤
    string enteredPassword = "사용자가 입력한 비밀번호"; // 사용자가 입력한 비밀번호를 저장하는 변수

    void Start()
    {
        // Photon 환경을 기반으로 접속을 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {

    }

    public void CreateRoom(string roomName, string password) 
    {
        RoomOptions roomOpt = new RoomOptions();
        roomOpt.IsVisible = false; // 비공개 방 설정
        roomOpt.IsOpen = true;
        roomOpt.MaxPlayers = 20;
        roomOpt.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "Password", password } };
        roomOpt.CustomRoomPropertiesForLobby = new string[] { "Password" };

        PhotonNetwork.CreateRoom(roomName, roomOpt);
    }
    public void JoinRoomWithPassword(string roomName, string enteredPassword)
    {
        // 방 정보를 찾기 위한 사용자 정의 룸 속성 검색
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
    }

    public void OnJoinRoom() 
    {
        print("성공적으로 방에 입장하였습니다!");

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Password")) 
        {
            string roomPassword = PhotonNetwork.CurrentRoom.CustomProperties["Password"].ToString();
            if(enteredPassword != roomPassword) 
            {
                print("비밀번호가 틀렸습니다!");
                PhotonNetwork.LeaveRoom(); // 잘못된 번호 입력 시 방을 나감
            }
            else 
            {
                print("입장을 환영합니다!");
            }
        }
    }

    public void CreatePersistentRoom(string roomName, string password) 
    {
        RoomOptions roomOpt = new RoomOptions();
        roomOpt.IsVisible = false;
        roomOpt.IsOpen = true;
        roomOpt.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "Password", password } };
        roomOpt.CustomRoomPropertiesForLobby = new string[] { "Password" };
        roomOpt.EmptyRoomTtl = 0; // 방이 비어 있어도 제거되지 않음
        roomOpt.PlayerTtl = -1; // 플레이어가 나가더라도 데이터 유지
    }


    /*// 마스터 서버에 접속이 되면 호출되는 함수
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
        PhotonNetwork.NickName = "태령";
        // 기본 Lobby 입장
        PhotonNetwork.JoinLobby();
    }

    // 로비에 참여를 성공하면 호출되는 함수
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 입장 완료");

        JoinOrCreateRoom();
    }

    // Room 을 참여하자. 만약에 해당 Room 이 없으면 Room 만들겠다.
    public void JoinOrCreateRoom()
    {
        // 방 생성 옵션
        RoomOptions roomOpt = new RoomOptions();

        roomOpt.MaxPlayers = 20; // 방에 들어올 수 있는 최대인원
        roomOpt.IsVisible = true; // 로비에 방을 보이게 할 지 
        roomOpt.IsOpen = true; // 방에 참여할 수 있는지

        // Room 참여 or 생성
        PhotonNetwork.JoinOrCreateRoom("meta_unity_room99", roomOpt, TypedLobby.Default);
    }

    // 방 생성이 성공했을 때 호출되는 함수
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료");
    }

    // 방 입장 성공했을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 입장 완료");

        // 멀티플레이 컨텐츠 즐길 수 있는 상태
        // GameScene으로 이동
        //PhotonNetwork.LoadLevel("GameScene");
    }*/
}
