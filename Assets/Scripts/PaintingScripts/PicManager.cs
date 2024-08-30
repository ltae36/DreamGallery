using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicManager : MonoBehaviour
{
    // 플레이어가 그린 그림들(텍스처)을 저장해서 불러온다.
    // ui에 차례대로 삽입해 표시된다.

    #region 배열로 넣기
    // 그림 목록
    public RawImage[] images;
    public List<Texture> paintings;

    private void Awake()
    {
        // 플레이하면 빈 캔버스 ui에 텍스처 배열을 집어넣는다.
        for (int i = 0; i < images.Length; i++)
        {
            paintings.Add(Resources.Load("pic" + i.ToString()) as Texture); // Resouces 폴더에서 그림들을 가져온다.

            //paintings[i] = Resources.Load("pic" + i.ToString()) as Texture; // Resouces 폴더에서 그림들을 가져온다.

            images[i].texture = paintings[i]; // Rawimages의 텍스처에 그림들을 넣는다.

            images[i].SetNativeSize(); // 그림 텍스처 크기에 Rawimage 크기를 맞춘다.
        }
    }
    #endregion
}
