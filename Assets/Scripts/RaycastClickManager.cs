using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastClickManager : MonoBehaviour
{
    // �׸��� �����ϴ� uiâ
    public GameObject selectPic;

    void Start()
    {
        selectPic.SetActive(false);
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        // �� ���ڸ� Ŭ���ϸ� �׸��� �� �� �ִ�.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) 
        {
            selectPic.SetActive(true);
        }

    }
}
