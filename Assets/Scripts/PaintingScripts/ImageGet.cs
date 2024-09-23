using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

public class ImageGet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string url = "https://7126-59-13-225-125.ngrok-free.app/get_image/pic11.png";
        StartCoroutine(DownloadImageFromServer(url));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DownloadImageFromServer(string imageUrl)
    {
        // UnityWebRequest를 사용해 GET 요청을 보냄
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);

        // 요청이 완료될 때까지 기다림
        yield return www.SendWebRequest();

        // 요청이 실패했는지 확인
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed to download image: {www.error}");
        }
        else
        {
            // 요청이 성공적으로 완료된 경우, Texture2D로 변환
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            // 텍스처가 성공적으로 다운로드 되었음을 출력
            Debug.Log("Image download successful!");

            // 다운로드한 텍스처를 UI 또는 오브젝트에 할당
            // 예를 들어, UI의 RawImage에 텍스처를 할당
            // yourRawImage.texture = texture;
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material.mainTexture = texture;
                Debug.Log("Texture applied to Quad!");
            }
            else
            {
                Debug.LogError("Quad does not have a MeshRenderer component.");
            }
        }
    }
}
