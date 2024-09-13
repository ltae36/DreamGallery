using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//// Delegate
//public delegate void CallBack();

public class PaintingModeMgr : MonoBehaviour
{
    //CallBack controllCan;

    // 캔버스를 하나 선택했다면 나머지는 작동하지 않는다.
    public GameObject[] canvas;
    public bool playerCheck;

    public bool canTransition;

    // 페인팅 모드 UI
    public GameObject paintingTool;

    private void Awake()
    {
        //controllCan = new CallBack[canvas.Length]; // 델리게이트 배열을 초기화한다.
        //for (int i = 0; i < canvas.Length; i++)
        //{
        //    pm = canvas[i].GetComponent<PaintingMode>();
        //}

        playerCheck = false;
        paintingTool.SetActive(false);
    }
    private void Start()
    {

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
            if (obj.tag == "Paint")
            {
                // 페인팅 툴 활성화
                paintingTool.SetActive(true);
                for (int i = 0; i < canvas.Length; i++)
                {
                    print(obj.name);
                    // 클릭한 캔버스와 배열의 캔버스들을 비교
                    if (obj.name != canvas[i].name)
                    {                        
                        // 클릭된 캔버스를 제외한 나머지를 모두 비활성화
                        if (playerCheck) canvas[i].SetActive(false);
                    }
                }
            }
        }
    }

    public void ClickStopButton()
    {
        // 그만그리기를 클릭하면 캔버스가 저장되고
        // 필드 화면으로 돌아간다.
        for (int i = 0; i < canvas.Length; i++)
        {
            // 캔버스들을 재활성화한다.
            canvas[i].SetActive(true);

            canTransition = true; // 전환을 시작하도록 플래그 설정
        }
    }
}
