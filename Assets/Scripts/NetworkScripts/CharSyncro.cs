using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharSyncro : MonoBehaviourPun
{
    // 멀티모드에서 방문자의 캐릭터를 생성한다.

    // 캐릭터 생성 위치
    public Transform startPos;



    void Start()
    {
        PhotonNetwork.Instantiate("Player", startPos.position, Quaternion.identity);
    }
}
