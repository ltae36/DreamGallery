using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastClickManager : MonoBehaviour
{
    // 그림을 선택하는 ui창
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
        // 빈 액자를 클릭하면 그림을 고를 수 있다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) 
        {
            selectPic.SetActive(true);
        }

    }
}
