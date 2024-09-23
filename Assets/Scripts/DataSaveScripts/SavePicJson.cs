using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PrefabData
{
    public string prefabName; // 프리팹 이름
    public Vector3 position; // 위치
    public Quaternion rotation; // 회전 정보

    public string materialName; // 머테리얼 이름
    public string texturePath; // 텍스처 경로

}


public class SavePicJson : MonoBehaviour
{
    public GameObject prefab;  // 프리팹
    private string savePath;   // 저장 경로

    void Start()
    {
        // JSON 저장 경로 설정
        savePath = Path.Combine(Application.persistentDataPath, "prefabData.json");
        LoadPrefab();
    }

    void Update()
    {

    }

    public void SavePrefab(GameObject instantiatePrefab)
    {
        // 저장된 게임오브젝트 메시렌더러의 머테리얼을 저장한다.
        MeshRenderer mr = instantiatePrefab.GetComponent<MeshRenderer>();
        Material material = mr.material;

        PrefabData data = new PrefabData
        {
            prefabName = instantiatePrefab.name,
            position = instantiatePrefab.transform.position,
            rotation = instantiatePrefab.transform.rotation,
            materialName = material.name,
            texturePath = material.mainTexture != null ? material.mainTexture.name : null  // 텍스처 정보 저장
        };

        // 데이터를 JSon으로 변환하여 저장
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    // 저장된 프리팹 정보를 불러오는 함수
    public void LoadPrefab()
    {
        if (File.Exists(savePath))
        {
            // JSON 파일에서 데이터 읽어오기
            string json = File.ReadAllText(savePath);
            PrefabData data = JsonUtility.FromJson<PrefabData>(json);

            // 프리팹을 저장된 위치와 회전으로 재생성
            GameObject instantiatedPrefab = Instantiate(prefab, data.position, data.rotation);
            instantiatedPrefab.name = data.prefabName;

            MeshRenderer renderer = instantiatedPrefab.GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                // Material 적용
                //Material material = Resources.Load<Material>(data.materialName);

                //StreamingAssets 폴더에서 텍스처를 불러온다.
                // Texture 적용 (있다면)
                //if (!string.IsNullOrEmpty(data.texturePath))
                //{
                //    Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(data.texturePath);
                //    if (texture != null)
                //    {
                //        renderer.material.mainTexture = texture;
                //    }
                //}
            }

            Debug.Log("Prefab 정보 불러오기 완료.");
        }
        else
        {
            Debug.LogWarning("저장된 데이터가 없습니다.");
        }
    }
}
