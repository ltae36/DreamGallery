using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BottonManager : MonoBehaviour
{
    // ������ ����
    public GameObject frame;

    public RaycastClickManager rcm;

    MeshRenderer mr;
    //public RawImage[] images;
    public Texture tex;
    GameObject clickObject;
    RawImage image;

    void Start()
    {
        mr = frame.GetComponent<MeshRenderer>();
    }

    void Update()
    {
        
    }

    public void ClickBotton()
    {
        clickObject = EventSystem.current.currentSelectedGameObject; // Ŭ���� �׸� ������Ʈ�� ����
        image = clickObject.GetComponent<RawImage>();
        tex = image.mainTexture;
        print(tex.name);

        // �ش��ϴ� �ڸ��� ���ڸ� �����Ѵ�.
        GameObject art = Instantiate(frame, rcm.blankFrame);

        // ���ڿ� ������ �׸��� �ִ´�.
        MeshRenderer mr = art.GetComponent<MeshRenderer>();
        mr.material.mainTexture = tex;

        art.transform.position = rcm.blankFrame.position;

        rcm.selectPic.SetActive(false);
    }
}
