using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PicManager : MonoBehaviour
{
    public List<Texture> textures = new List<Texture>(); // 텍스처를 저장할 리스트
    public List<Sprite> sprites = new List<Sprite>();  // 스프라이트를 저장할 리스트    
    public Image[] thumNails; // 선택창에 표시될 이미지

    // 저장 버튼을 누르면 그림 목록을 다시 불러온다.

    void Start()
    {
    }

    // StreamingAsset 폴더에서 이미지 파일 가져오는 함수
    //public void GetStreamingAssetImage() 
    //{
    //    // StreamingAssets 경로
    //    string path = Application.streamingAssetsPath;

    //    // 해당 폴더의 모든 PNG 파일 경로 불러오기
    //    string[] filePaths = Directory.GetFiles(path, "*.png");

    //    foreach (string filePath in filePaths)
    //    {
    //        string url = "file://" + filePath; // png파일 경로를 파일명에 합치기

    //        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
    //        {
    //            yield return www.SendWebRequest();

    //            Texture2D texture = DownloadHandlerTexture.GetContent(www);

    //            textures.Add(texture); // 텍스처 리스트에 추가               
    //        }
    //    }
    //}

    // 게임이 시작하면 StreamingAsset 폴더에서 이미지 파일들을 가져온다.
    public IEnumerator LoadSprites(System.Action onComplete)
    {
        // StreamingAssets 경로
        string path = Application.streamingAssetsPath;

        // 해당 폴더의 모든 PNG 파일 경로 불러오기
        string[] filePaths = Directory.GetFiles(path, "*.png");

        foreach (string filePath in filePaths)
        {
            string url = "file://" + filePath; // png파일 경로를 파일명에 합치기

            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
            {
                yield return www.SendWebRequest();

                Texture2D texture = DownloadHandlerTexture.GetContent(www);

                textures.Add(texture); // 텍스처 리스트에 추가

                Sprite sprite = TextureToSprite(texture);
                sprites.Add(sprite);  // 스프라이트 리스트에 추가

                // 이미지 썸네일에 스프라이트를 추가한다.
                for (int i = 0; i < sprites.Count - 1; i++)
                {
                    thumNails[i].sprite = sprites[i];

                    // 사이즈를 이미지 사이즈로 맞춘다.
                    thumNails[i].SetNativeSize();
                }
                #region www에러코드
                //if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                //{
                //    Debug.LogError("Failed to load texture: " + www.error);
                //}
                //else
                //{
                //    Texture2D texture = DownloadHandlerTexture.GetContent(www); 
                //    Sprite sprite = TextureToSprite(texture);
                //    sprites.Add(sprite);  // 스프라이트 리스트에 추가

                //    // 이미지 썸네일에 스프라이트를 추가한다.
                //    for (int i = 0; i < sprites.Count - 1; i++)
                //    {
                //        thumNails[i].sprite = sprites[i];

                //        // 사이즈를 이미지 사이즈로 맞춘다.
                //        thumNails[i].SetNativeSize();
                //    }
                //}
                #endregion
            }
        }

        // 코루틴 완료 후 콜백 실행
        onComplete?.Invoke();

        print("그림 불러오기 완료!");
    }

    // Texture2D를 Sprite로 변환하는 헬퍼 함수
    private Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
