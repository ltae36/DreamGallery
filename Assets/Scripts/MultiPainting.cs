using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiPainting : MonoBehaviourPun
{

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) OnMouseUp();
    }

    private void OnMouseUp()
    {
        // 마우스 포인터 위치에 레이를 쏜다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            print(hit.collider.gameObject.name);
            // 캔버스를 클릭하면 멀티그리기 씬으로 넘어간다.
            if (hit.collider.gameObject.tag == "Paint")
            {
                PhotonNetwork.LoadLevel("PaintingSceneMulti");
            }
        }
    }
}
