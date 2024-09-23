using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //친구의 아이디를 입력해 친구의 갤러리에 방문한다.

    // 방제입력
    public TMP_InputField inputRoomCode;
    // 방찾기용 입력
    public TMP_InputField searchRoomCode;
    // 인원수 입력
    public TMP_InputField maxPlayer;

    // 생성 / 참가 버튼
    public Button createRoom;
    public Button joinRoom;


    void Start()
    {
        // 로비 진입
        //PhotonNetwork.JoinLobby();

        // 인풋필드의 내용이 변경되었을 때 호출되는 함수
        inputRoomCode.onValueChanged.AddListener(OnValueChangedRoomName);
        searchRoomCode.onValueChanged.AddListener(OnValueChangedRoomName);
        maxPlayer.onValueChanged.AddListener(OnValueChangedMaxPlayer);
    }

    // 입력에 따른 버튼 활성화
    void OnValueChangedRoomName(string roomName) 
    {
        // 방제가 입력되었을 때
        createRoom.interactable = roomName.Length > 0 && maxPlayer.text.Length > 0;
        joinRoom.interactable = roomName.Length > 0;
    }
    void OnValueChangedMaxPlayer(string maxPlayer) 
    {
        // 초대 인원 수 가 입력되었을 때
        createRoom.interactable = maxPlayer.Length > 0 && inputRoomCode.text.Length > 0;
    }

    #region 방 생성
    public void CreateRoom() 
    {
        // 방 옵션 설정
        RoomOptions option = new RoomOptions();
        // 공개 여부 설정
        option.IsVisible = false;
        // 최대 인원 설정
        option.MaxPlayers = int.Parse(maxPlayer.text);
        // 방을 생성
        PhotonNetwork.CreateRoom(inputRoomCode.text, option);
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
        // 방 입장 요청
        PhotonNetwork.JoinRoom(searchRoomCode.text);
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
