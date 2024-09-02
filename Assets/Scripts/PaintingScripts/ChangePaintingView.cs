using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePaintingView : MonoBehaviour
{
    // 캔버스 클릭하면 페인팅 화면 전환
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
        // 마우스 포인터 위치에 레이를 쏜다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            print(hit.collider.gameObject.name);
            // 캔버스를 클릭하면 카메라가 전환된다.
            if (hit.collider.gameObject.name == "PaintngCanvas")
            {
                globalView.SetActive(false);
                paintingView.enabled = true;
            }
        }
    }
}
