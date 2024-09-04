﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BottonManager : MonoBehaviour
{
    // 생성할 액자
    public GameObject frame;
    public RaycastClickManager rcm;
    public PicManager pm;

    MeshRenderer mr;
    //public RawImage[] images;
    
    GameObject clickObject;
    RawImage image;
    Texture tex;

    void Start()
    {
        mr = frame.GetComponent<MeshRenderer>();
        
    }

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

        // 해당하는 자리에 액자를 생성한다.
        GameObject art = Instantiate(frame, rcm.blankFrame);

        // 액자에 선택한 그림을 넣는다.
        MeshRenderer mr = art.GetComponent<MeshRenderer>();
        mr.material.mainTexture = tex;

        art.transform.position = rcm.blankFrame.position;

        rcm.selectPic.SetActive(false);
    }
}
