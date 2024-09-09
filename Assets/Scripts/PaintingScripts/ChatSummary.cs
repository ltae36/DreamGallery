using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChatSummary : MonoBehaviour
{
    public string url; // 서버 URL을 저장하는 변수
    public Text result_text; // 서버 응답을 출력할 UI 텍스트

    // 서버로부터 GET 요청을 통해 데이터를 받아오는 메서드
    public void GetDataFromServer()
    {
        StartCoroutine(GetRequest(url)); // 코루틴을 시작하여 GET 요청을 처리
    }

    // 실제로 GET 요청을 보내는 코루틴 메서드
    IEnumerator GetRequest(string url)
    {
        // GET 요청을 보내기 위한 UnityWebRequest 객체 생성
        UnityWebRequest request = UnityWebRequest.Get(url);

        // 서버에 GET 요청을 보내고 응답을 기다림
        yield return request.SendWebRequest();

        // 응답이 성공적으로 완료되었는지 확인
        if (request.result == UnityWebRequest.Result.Success)
        {
            // 서버로부터 받은 응답 데이터를 텍스트로 변환하여 출력
            string response = request.downloadHandler.text;
            Debug.Log("서버 응답: " + response);

            // 서버에서 받은 JSON 텍스트 데이터를 Unity UI에 표시
            result_text.text = response;
        }
        else
        {
            // 요청이 실패한 경우 오류 메시지를 출력
            Debug.LogError("Error: " + request.error);
            result_text.text = "Error: " + request.error;
        }
    }
}
