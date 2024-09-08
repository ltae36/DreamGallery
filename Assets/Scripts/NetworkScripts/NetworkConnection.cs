using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkConnection : MonoBehaviourPunCallbacks
{
    // 갤러리에서 온라인 모드를 켜면 방이 열리고 초대할 수 있음
    // 방은 자동으로 생성되고 보이게 할 지, 초대 할 지를 플레이어가 컨트롤
    public GameObject mychar;

    void Start()
    {
        if(PhotonNetwork.IsConnected) mychar.SetActive(false);
    }

    public void OnClickConnect() 
    {

        // 온라인 모드 버튼을 누르면 마스터 서버에 접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버에 접속이 되면 호출되는 함수
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();


        // 아이디 설정
        int charID = Random.Range(100, 999);
        PhotonNetwork.NickName = "꿈빛화가" + charID;

        // 초대 설정 창으로 이동
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}
