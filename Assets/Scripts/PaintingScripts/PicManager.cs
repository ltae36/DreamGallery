using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicManager : MonoBehaviour
{
    // �÷��̾ �׸� �׸���(�ؽ�ó)�� �����ؼ� �ҷ��´�.
    // ui�� ���ʴ�� ���Եȴ�.

    // �׸� ���
    public RawImage[] images;
    public Texture[] paintings;
    public GameObject[] arts;


    void Start()
    {
        // �÷����ϸ� �� ĵ���� ui�� �ؽ�ó �迭�� ����ִ´�.
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
