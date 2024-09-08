using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;   // HTTP 통신을 위한 네임스페이스
using System.Text;              // JSON, CSV 같은 문서 형태의 인코딩(UTF-8)을 위한 네임스페이스
using UnityEngine.UI;           // UI 관련 클래스 사용을 위한 네임스페이스
using System;                   // 기본 시스템 기능을 위한 네임스페이스 (예: Convert 클래스)
using System.IO;                // 파일 I/O를 위한 네임스페이스
using UnityEditor;

public class HttpManager : MonoBehaviour
{

    public string url; // 서버 URL을 저장하는 변수
    private string userString; // 사용자가 입력한 문자열을 저장하는 변수
    public Text result_text;
    public Text userInput;
    public Button postButton;
    public GameObject loading;

    // 사용자가 입력한 문자열을 JSON으로 변환하고 POST 요청을 보내는 메서드
    public void PostString()
    {
        userString = userInput.text;

       // loading.SetActive(true);
        postButton.interactable = false;

        StartCoroutine(PostStringRequest(url, userString)); // 코루틴을 시작하여 POST 요청을 처리
    }

    // 실제로 POST 요청을 보내는 코루틴 메서드
    IEnumerator PostStringRequest(string url, string userString)
    {
        // 사용자가 입력한 문자열을 StringData 구조체로 감싸고, 이를 JSON 형식으로 변환
        StringData data = new StringData();
        data.content = userString;
        string jsonData = JsonUtility.ToJson(data);

        // JSON 데이터를 바이트 배열로 인코딩 (UTF-8)
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

        // HTTP POST 요청을 설정
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();

        // 서버에 POST 요청을 보내고 응답을 기다림
        yield return request.SendWebRequest();

        // 응답이 성공적으로 완료되었는지 확인
        if (request.result == UnityWebRequest.Result.Success)
        {
            // 서버로부터 받은 응답 데이터를 텍스트로 변환하여 출력
            string response = request.downloadHandler.text;

            ReceiveData receiveData = JsonUtility.FromJson<ReceiveData>(response);
            result_text.text = receiveData.result;
        
        }
        else
        {
            // 요청이 실패한 경우 오류 메시지를 출력
            Debug.LogError("Error: " + request.error);
            result_text.text = "Error: " + request.error;
        }
        userInput.text = "";
        postButton.interactable = true;
       // loading.SetActive(false);
    }
}

// 사용자가 입력한 문자열을 담기 위한 구조체
[System.Serializable]
public struct StringData
{
    public string content; // 사용자가 입력한 문자열을 저장하는 필드
}

[System.Serializable]
public struct ReceiveData
{
    public string result; // 서버에서 받은 텍스트 데이터
}
