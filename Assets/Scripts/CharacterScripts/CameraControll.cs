using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviourPun
{
    // 카메라 
    public GameObject cam;

    void Start()
    {
        // 내 것일 때만 카메라를 활성화자
        cam.SetActive(photonView.IsMine);
    }

}
