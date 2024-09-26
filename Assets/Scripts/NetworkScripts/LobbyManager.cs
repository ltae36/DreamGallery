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

    // 방찾기용 입력
    public TMP_InputField searchRoomCode;

    //참가 버튼
    public Button joinRoom;


    void Start()
    {
        // 로비 진입
        //PhotonNetwork.JoinLobby();

        // 인풋필드의 내용이 변경되었을 때 호출되는 함수
        searchRoomCode.onValueChanged.AddListener(OnValueChangedRoomName);
    }

    // 입력에 따른 버튼 활성화
    void OnValueChangedRoomName(string roomName) 
    {
        // 방제가 입력되었을 때
        joinRoom.interactable = roomName.Length > 0;
    }

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
