using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicManager : MonoBehaviour
{
    // 플레이어가 그린 그림들(텍스처)을 저장해서 불러온다.
    // ui에 차례대로 삽입된다.

    // 그림 목록
    public RawImage[] images;
    public Texture[] paintings;
    public GameObject[] arts;


    void Start()
    {
        // 플레이하면 빈 캔버스 ui에 텍스처 배열을 집어넣는다.
        for(int i = 0; i < paintings.Length; i++) 
        {
            images[i].texture = paintings[i];
            images[i].SetNativeSize();
        }
    }

    void Update()
    {
        
    }

}
