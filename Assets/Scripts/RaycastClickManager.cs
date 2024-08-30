using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RaycastClickManager : MonoBehaviour
{
    // �׸��� �����ϴ� UI
    public GameObject selectPic;

    // ���ڸ� ��ġ(���ڸ� �� �ڸ�)
    public Transform blankFrame;    

    void Start()
    {
        // �׸�����â ��Ȱ��ȭ
        selectPic.SetActive(false);
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

    // �� �ڸ��� Ŭ���ϸ� �׸� ����â�� �߰� �� �׸��� �� ���ڸ� �ش� �ڸ��� �� �� �ִ�.
    private void OnMouseDown()
    {
        // ���콺 ������ ��ġ�� ���̸� ���.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Ŭ���� ������Ʈ�� ����
            GameObject obj = hit.collider.gameObject;
            Debug.Log(obj.name);
            // Ŭ���� ������Ʈ�� ���ڸ����
            if (obj.tag == "EmptyFrame") 
            {
                // �׸� ����â�� Ȱ��ȭ�ȴ�.
                selectPic.SetActive(true);
                // ������ �ڸ��� ��ġ�� �����Ѵ�.
                blankFrame = obj.transform;
                // ���ڸ� ǥ�ø� ���ش�.
                MeshRenderer[] objMeshRenderer = obj.GetComponents<MeshRenderer>();
                Collider objCollider = obj.GetComponent<Collider>();

                objMeshRenderer[0].enabled = false;
                objCollider.enabled = false;
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
