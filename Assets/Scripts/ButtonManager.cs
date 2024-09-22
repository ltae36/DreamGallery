using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    // 생성할 액자
    public GameObject planeW;
    public GameObject planeH;

    // 액자 클릭 스크립트
    public RaycastClickManager rcm;

    // 텍스처, 스프라이트 및 썸네일 목록 스크립트
    public PicManager pm;

    // 저장 유틸리티
    //public SavePicJson saveManager;

    MeshRenderer mr;    
    GameObject clickObject;
    Texture tex;

    GameObject art;


    void Start()
    {
        // 그림을 넣을 플레인의 메쉬렌더러 컴포넌트를 가져온다.
        mr = planeW.GetComponentInChildren<MeshRenderer>();
        mr = planeH.GetComponentInChildren<MeshRenderer>();        
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R)) 
        //{
        //    saveManager.SavePrefab(art);
        //}

        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    saveManager.LoadPrefab();  // 저장된 프리팹 정보를 불러와 생성
        //}
    }

    // 그림 선택창에서 그림을 클릭하면 불러오는 함수
    public void ClickBotton()
    {
        // 클릭한 오브젝트의 인덱스의 번호를 저장하여 해당하는 번호의 텍스처를 저장하여 가져온다.
        string selectIndex = EventSystem.current.currentSelectedGameObject.name;
        int index = int.Parse(selectIndex);
        print(index);
        tex = pm.textures[index];

        //clickObject = EventSystem.current.currentSelectedGameObject; // 클릭한 그림 오브젝트를 저장
        //image = clickObject.GetComponent<RawImage>();
        //tex = image.mainTexture;
        //print(tex.name);

        // 플레인 프리팹 생성
        if (rcm.checkWH) // 가로 액자를 선택했을 경우
        {
            art = Instantiate(planeW, rcm.blankFrame);
        }
        else // 세로 액자를 선택했을 경우
        {
            art = Instantiate(planeH, rcm.blankFrame);
        }

        // 액자에 선택한 그림을 넣는다.
        MeshRenderer mr = art.GetComponentInChildren<MeshRenderer>();
        mr.material.mainTexture = tex;

        // 생성한 그림의 위치를 액자 위로 맞춘다.
        art.transform.position = rcm.blankFrame.position;

        // 그림 선택창을 비활성화
        rcm.selectPic.SetActive(false);
    }
}
