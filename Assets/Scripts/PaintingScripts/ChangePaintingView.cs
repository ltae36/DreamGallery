using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePaintingView : MonoBehaviour
{
    // ĵ���� Ŭ���ϸ� ������ ȭ�� ��ȯ
    public Camera paintingView;
    public GameObject globalView;

    void Start()
    {
        paintingView.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            OnMouseDown();
        }
    }

    private void OnMouseDown()
    {
        // ���콺 ������ ��ġ�� ���̸� ���.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            print(hit.collider.gameObject.name);
            // ĵ������ Ŭ���ϸ� ī�޶� ��ȯ�ȴ�.
            if (hit.collider.gameObject.name == "PaintngCanvas")
            {
                globalView.SetActive(false);
                paintingView.enabled = true;
            }
        }
    }
}
