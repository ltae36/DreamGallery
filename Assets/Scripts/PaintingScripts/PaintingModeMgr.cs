using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingModeMgr : MonoBehaviour
{
    // 캔버스를 하나 선택했다면 나머지는 작동하지 않는다.
    public GameObject[] canvas;
    public bool playerCheck;

    private void Start()
    {
        playerCheck = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) OnMouseDown();                
    }

    private void OnMouseDown()
    {
        // 마우스 포인터 위치에 레이를 쏜다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // 클릭한 오브젝트를 저장
            GameObject obj = hit.collider.gameObject;
            for(int i = 0; i < canvas.Length; i++) 
            {
                print(obj.name);
                // 클릭한 캔버스와 배열의 캔버스들을 비교
                if(obj.name != canvas[i].name) 
                {
                    // 클릭된 캔버스를 제외한 나머지를 모두 비활성화
                    if (playerCheck) canvas[i].SetActive(false);                    
                }
            }
        }
    }
}
