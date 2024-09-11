using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RaycastClickManager : MonoBehaviour
{
    // 빈 액자를 클릭하면 그림 선택창이 뜨고 고른 그림이 들어간 액자를 해당 자리에 걸 수 있다.

    // 그림을 선택하는 UI
    public GameObject selectPic;

    // 빈자리 위치(액자를 걸 자리)
    public Transform blankFrame;

    public bool checkWH; // 가로가 true, 세로가 false

    void Start()
    {
        // 그림선택창 비활성화
        selectPic.SetActive(false);
    }

    void Update()
    {
        #region 모바일 터치 입력
        // 터치 입력이 있는지 확인한다.
        if (Input.touchCount > 0) 
        {
            // 첫번째 터치를 가져온다.
            Touch touch = Input.GetTouch(0);

            // 터치가 시작되었는지 확인한다.( = GetMouseButtonDown)
            if(touch.phase == TouchPhase.Began) 
            {
                OnTouchDown(touch);
            }
        }
        #endregion

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
            // 클릭한 오브젝트를 저장
            GameObject obj = hit.collider.gameObject;
            Debug.Log(obj.name);
            // 클릭한 오브젝트가 빈자리라면
            if (obj.tag == "FrameW" || obj.tag == "FrameH") 
            {
                if (obj.tag == "FrameW") checkWH = true;
                else if (obj.tag == "FrameH") checkWH = false;
                // 그림 선택창이 활성화된다.
                selectPic.SetActive(true);
                // 선택한 자리의 위치를 저장한다.
                blankFrame = obj.transform;
                Collider objCol = obj.GetComponent<Collider>();

                // 빈자리 표시를 없앤다.
                objCol.enabled = false;
            }            
        }
    }

    private void OnTouchDown(Touch touch)
    {
        // 빈 액자를 클릭하면 그림을 고를 수 있다.
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) 
        {
            string objectName = hit.collider.gameObject.name;
            Debug.Log(objectName);
        }
    }


}
