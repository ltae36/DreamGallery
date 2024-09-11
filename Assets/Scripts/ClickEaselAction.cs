using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEaselAction : MonoBehaviour
{
    // 이젤을 클릭했을 때 
    // 아웃라인이 핑크색으로 바뀐다.
    Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) OnMouseDown();
        else 
        {
            outline.OutlineWidth = 0.2f;
            outline.OutlineColor = Color.white;
        }
    }

    private void OnMouseDown()
    {
        // 마우스 포인터 위치에 레이를 쏜다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            outline = hit.collider.gameObject.GetComponent<Outline>();
            // 캔버스를 클릭하면 카메라가 전환된다.
            if(outline != null) 
            {
                outline.OutlineWidth = 5.5f;
                outline.OutlineColor = Color.magenta;
            }
        }
    }
}
