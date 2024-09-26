using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HttpGalleryMgr : MonoBehaviour
{
    // 갤러리의 저장 정보를 서버에 POST하여 저장하고
    // 재실행하면 저장 정보를 서버에서 GET하여 로딩한다.
    public string url;
    public ButtonManager buttonManager;

    void Start()
    {
        
    }

    public void Get()
    {
        StartCoroutine(GetRequest(url));
    }

    // Get 통신 코루틴 함수
    IEnumerator GetRequest(string url)
    {
        // http Get 통신 준비를 한다.
        UnityWebRequest request = UnityWebRequest.Get(url);

        // 서버에 Get 요청을 하고, 서버로부터 응답이 올 때까지 대기한다.
        yield return request.SendWebRequest();

        // 만일, 서버로부터 온 응답이 성공(200)이라면...
        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonData = request.downloadHandler.text;
            SaveJsonData(jsonData);

            // json데이터에서 갤러리 정보를 불러온다
            buttonManager.LoadGallery();

            print("갤러리 복원 성공!");
        }
        // 그렇지 않다면(400, 404 etc)...
        else
        {
            // 에러 내용을 출력한다.
            print(request.error);
        }

    }

    void SaveJsonData(string jsonData) 
    {
        string path = buttonManager.saveFilePath;

        // 파일에 json데이터 덮어쓰기
        File.WriteAllText(path, jsonData);
    }

    // 서버에 갤러리 데이터를 Post하는 함수
    public void PostJson() 
    {
        StartCoroutine(PostJsonRequest(url));
    }

    IEnumerator PostJsonRequest(string url) 
    {
        string filePath = buttonManager.saveFilePath;

        if (File.Exists(filePath)) 
        {
            string jsonData = File.ReadAllText(filePath);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);
            //// JSON 데이터를 바이트 배열로 인코딩 (UTF-8)
            //FrameData userData = new FrameData();
            //string userJsonData = JsonUtility.ToJson(userData, true);
            //byte[] jsonBytes = Encoding.UTF8.GetBytes(userJsonData);

            // Post를 하기 위한 준비를 한다.
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();

            // 서버에 POST 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // 다운로드 핸들러에서 텍스트 값을 받아서 UI에 출력한다.
                string response = request.downloadHandler.text;
                Debug.LogWarning(response);
            }
            else
            {
                Debug.LogError(request.error);
            }
        }       

    }
}
