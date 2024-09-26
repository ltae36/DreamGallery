using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    // multi모드에서 플레이어의 카메라를 메인카메라로 변경
    public GameObject player;
    GameObject camera;
    public GameObject paintCamera;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        camera = player.transform.GetChild(2).gameObject;
        camera.SetActive(false);
        paintCamera.transform.parent = player.transform;
    }

    void Update()
    {
        
    }
}
