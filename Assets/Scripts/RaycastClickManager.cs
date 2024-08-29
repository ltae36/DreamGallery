using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastClickManager : MonoBehaviour
{
    // 그림을 선택하는 ui창
    public GameObject selectPic;
    public PicManager pm;

    MeshRenderer mr;

    public Texture[] tx;

    FramePainting fp;

    void Start()
    {
        selectPic.SetActive(false);

        tx = pm.paintings; // picManager의 그림들을 가져옴
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
        // 빈 자리를 클릭하면 그림 선택창이 뜨고 고른 그림이 들어간 액자를 해당 자리에 걸 수 있다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            string objectName = hit.collider.gameObject.name;
            Debug.Log(objectName);
            if (objectName == "Frame") 
            {
                selectPic.SetActive(true);
                GameObject go = hit.collider.gameObject; // 선택된 게임오브젝트를 가져온다.
                //fp = gameObject.GetComponent<FramePainting>();
                //mr = go.GetComponent<MeshRenderer>();
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
