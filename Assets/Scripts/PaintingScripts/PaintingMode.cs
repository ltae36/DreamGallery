using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintingMode : MonoBehaviour
{
    // 캔버스 클릭하면 페인팅 화면 전환

    // 그림그리는 화면
    public GameObject paintingView;
    public Transform paintingMode;

    // 필드 화면    
    public GameObject globalView;
    // 페인팅 UI
    public GameObject paintingTool;

    private bool shouldTransition = false;
    private float transitionProgress = 0f;
    public float transitionSpeed = 2f; // 이동 속도를 조정할 수 있는 변수

    void Start()
    {
        paintingView.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) OnMouseDown();
        if (shouldTransition)
        {
            // 카메라 위치와 회전 전환
            transitionProgress += Time.deltaTime * transitionSpeed;
            paintingView.transform.position = Vector3.Lerp(globalView.transform.position, paintingMode.position, transitionProgress);
            paintingView.transform.rotation = Quaternion.Lerp(globalView.transform.rotation, paintingMode.rotation, transitionProgress);

            if (transitionProgress >= 1f)
            {
                shouldTransition = false;
                transitionProgress = 0f; // 전환 완료 후 초기화
            }
        }
    }

    private void OnMouseDown()
    {
        // 마우스 포인터 위치에 레이를 쏜다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            print(hit.collider.gameObject.name);
            // 캔버스를 클릭하면 카메라가 전환된다.
            if (hit.collider.gameObject.tag == "Paint")
            {
                paintingView.SetActive(true);
                shouldTransition = true; // 전환을 시작하도록 플래그 설정
                paintingTool.SetActive(true);
            }
        }
    }
}
