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

    //GameObject clickObject;
    //Texture tex;

    string saveFilePath;
    GameObject art;

    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/galleryData.json";
        LoadGallery();

        //// 그림을 넣을 플레인의 메쉬렌더러 컴포넌트를 가져온다.
        //mr = planeW.GetComponentInChildren<MeshRenderer>();
        //mr = planeH.GetComponentInChildren<MeshRenderer>();        
    }

    // 그림 선택창에서 그림을 클릭하면 불러오는 함수
    public void ClickBotton()
    {
        // 클릭한 오브젝트의 인덱스의 번호를 저장하여 해당하는 번호의 텍스처를 저장하여 가져온다.
        string selectIndex = EventSystem.current.currentSelectedGameObject.name;
        int index = int.Parse(selectIndex);
        Texture tex = pm.textures[index];

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

        // 저장한다
        SaveFrameData(art.transform.position, index, rcm.checkWH);

        // 그림 선택창을 비활성화
        rcm.selectPic.SetActive(false);
    }

    // 데이터를 저장하는 함수
    void SaveFrameData(Vector3 framePosition, int textureIndex, bool isHorizontal) 
    {
        GalleryData gallery = LoadGalleryData();

        FrameData newFrameData = new FrameData
        {
            pos = framePosition,
            texturePath = "texture_" + textureIndex,
            isHorizontal = isHorizontal
        };
        gallery.frames.Add(newFrameData);

        // Json변환 및 저장
        string json = JsonUtility.ToJson(gallery);
        System.IO.File.WriteAllText(saveFilePath, json);
    }

    // 갤러리 데이터를 로드하는 함수
    private GalleryData LoadGalleryData()
    {
        if (System.IO.File.Exists(saveFilePath))
        {
            string json = System.IO.File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<GalleryData>(json);
        }
        return new GalleryData(); // 데이터가 없으면 새로 생성
    }

    // 저장된 갤러리 로드 및 복원
    private void LoadGallery()
    {
        GalleryData galleryData = LoadGalleryData();

        foreach (FrameData frameData in galleryData.frames)
        {
            GameObject newFrame;
            if (frameData.isHorizontal)
            {
                newFrame = Instantiate(planeW, frameData.pos, Quaternion.identity);
            }
            else
            {
                newFrame = Instantiate(planeH, frameData.pos, Quaternion.identity);
            }

            int textureIndex = int.Parse(frameData.texturePath.Replace("texture_", ""));
            newFrame.GetComponentInChildren<MeshRenderer>().material.mainTexture = pm.textures[textureIndex];
        }
    }
}

[System.Serializable]
public class FrameData 
{
    public Vector3 pos; // 그림을 건 액자의 위치
    public string texturePath; // 그림의 텍스처인덱스
    public bool isHorizontal; // 가로세로 구분
}

[System.Serializable]
public class GalleryData
{
    public List<FrameData> frames = new List<FrameData>(); // 여러 액자의 정보 저장
}