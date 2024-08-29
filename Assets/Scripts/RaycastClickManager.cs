using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastClickManager : MonoBehaviour
{
    // �׸��� �����ϴ� uiâ
    public GameObject selectPic;
    public PicManager pm;

    MeshRenderer mr;

    public Texture[] tx;

    FramePainting fp;

    void Start()
    {
        selectPic.SetActive(false);

        tx = pm.paintings; // picManager�� �׸����� ������
    }

    void Update()
    {
        #region ����� ��ġ �Է�
        // ��ġ �Է��� �ִ��� Ȯ���Ѵ�.
        if (Input.touchCount > 0) 
        {
            // ù��° ��ġ�� �����´�.
            Touch touch = Input.GetTouch(0);

            // ��ġ�� ���۵Ǿ����� Ȯ���Ѵ�.( = GetMouseButtonDown)
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
        // �� �ڸ��� Ŭ���ϸ� �׸� ����â�� �߰� �� �׸��� �� ���ڸ� �ش� �ڸ��� �� �� �ִ�.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            string objectName = hit.collider.gameObject.name;
            Debug.Log(objectName);
            if (objectName == "Frame") 
            {
                selectPic.SetActive(true);
                GameObject go = hit.collider.gameObject; // ���õ� ���ӿ�����Ʈ�� �����´�.
                //fp = gameObject.GetComponent<FramePainting>();
                //mr = go.GetComponent<MeshRenderer>();
            }            
        }
    }

    private void OnTouchDown(Touch touch)
    {
        // �� ���ڸ� Ŭ���ϸ� �׸��� �� �� �ִ�.
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) 
        {
            string objectName = hit.collider.gameObject.name;
            Debug.Log(objectName);
        }
    }
}
