using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintingMode : MonoBehaviour
{
    // 캔버스를 클릭하면 페인팅 화면으로 전환된다.

    // 그림그리는 화면
    GameObject paintingView;
    Transform paintingMode;

    // 필드 뷰 카메라
    GameObject globalView;

    // 플레이어 캐릭터
    GameObject player;

    // 페인팅모드 매니저
    PaintingModeMgr pmm;

    private bool shouldTransition = false;
    private float transitionProgress = 0f;
    public float transitionSpeed = 2f; // 이동 속도를 조정할 수 있는 변수

    private void Awake()
    {
        paintingView = transform.GetChild(0).gameObject;
        paintingMode = transform.GetChild(1).gameObject.transform;
        paintingView.SetActive(false);

        globalView = GameObject.FindWithTag("MainCamera");
        player = GameObject.FindWithTag("Player");
        GameObject pmmObj = GameObject.Find("ModeManager");
        pmm = pmmObj.GetComponent<PaintingModeMgr>();
    }
    void Start()
    {        

    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) OnMouseUp();
        if (shouldTransition)
        {
            TransitionCamera();
            player.SetActive(false);
        }

        if (pmm.canTransition) 
        {
            TransitionCameraBack();
            player.SetActive(true);
            StartCoroutine(WaitCameraOff());
        }
    }

    private void OnMouseUp()
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
            }
        }
    }

    // 캔버스를 클릭하면 그림그리기 화면으로 전환된다
    void TransitionCamera()
    {
        player.SetActive(false);

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

    // 그만하기를 클릭하면 다시 필드 뷰로 전환된다.
    public void TransitionCameraBack()
    {
        player.SetActive(true);

        // 카메라 위치와 회전 전환
        transitionProgress += Time.deltaTime * transitionSpeed;
        paintingView.transform.position = Vector3.Lerp(paintingMode.position, globalView.transform.position, transitionProgress);
        paintingView.transform.rotation = Quaternion.Lerp(paintingMode.rotation, globalView.transform.rotation, transitionProgress);

        if (transitionProgress >= 1f)
        {
            pmm.canTransition = false;
            transitionProgress = 0f; // 전환 완료 후 초기화
        }
    }

    IEnumerator WaitCameraOff()
    {
        yield return new WaitForEndOfFrame();
        paintingView.SetActive(false);
    }
}
