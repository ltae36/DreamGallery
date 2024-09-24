//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;
//using UnityEngine.Playables;

//public class SaveJson : MonoBehaviour
//{
//    public GameObject picPrefab;
//    private string saveFilePath;

//    private void Start()
//    {
//        saveFilePath = Application.persistentDataPath + "/galleryData.json";

//        // 게임 시작 시 기존 저장 데이터를 불러와 갤러리 복원
//        LoadGallery();
//    }

//    void SavePaintingData(Vector3 framePosition, string textureFileName) 
//    {
//        // 저장할 데이터를 로드
//        GalleryData galleryData = LoadGalleryData();

//        PaintingData newPaintingData = new PaintingData 
//        {
//            pos = framePosition,
//            texturePath = textureFileName
//        };
//        galleryData.frames.Add(newPaintingData);

//        // Json으로 변환 저장
//        string json = JsonUtility.ToJson(galleryData);
//        File.WriteAllText(saveFilePath, json);
//    }

//    // 저장된 데이터를 불러오는 함수
//    private GalleryData LoadGalleryData() 
//    {
//        if (File.Exists(saveFilePath)) 
//        {
//            string json = File.ReadAllText(saveFilePath);
//            return JsonUtility.FromJson<GalleryData>(json);
//        }
//        else 
//        {
//            return new GalleryData(); // 새 데이터를 생성
//        }
//    }

//    private Texture2D LoadTextureFromFile(string filePath)
//    {
//        byte[] fileData = File.ReadAllBytes(filePath);
//        Texture2D texture = new Texture2D(2, 2);
//        texture.LoadImage(fileData); // 텍스처 로드
//        return texture;
//    }

//    // 갤러리를 불러오고 복원하는 함수
//    private void LoadGallery()
//    {
//        GalleryData gallery = LoadGalleryData();

//        foreach (PaintingData paintingData in gallery.frames) 
//        {
//            // 저장된 위치에 프리팹 생성
//            GameObject newPainting = Instantiate(picPrefab, paintingData.pos, Quaternion.identity);

//            // 텍스처를 StreamingAssets에서 로드
//            string texturePath = Path.Combine(Application.streamingAssetsPath, paintingData.texturePath);
//            Texture2D texture = LoadTextureFromFile(texturePath);
//        }
//    }
//}

//[System.Serializable]
//public class PaintingData 
//{    
//    public Vector3 pos; // 그림을 건 액자의 위치
//    public string texturePath; // 그림의 텍스처인덱스
//    public bool isHorizontal; // 가로세로 구분
//}

////[System.Serializable]
////public class GalleryData 
////{
////    public List<PaintingData> frames = new List<PaintingData>(); // 여러 액자의 정보 저장
////}
