using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpSpace : MonoBehaviour
{
    // 트리거에 진입하면 해당 장소로 이동한다.
    public GameObject moveButton;

    Animator anim;
    

    void Start()
    {
        moveButton.SetActive(false);

        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        // 문 앞에 플레이어가 있는 것을 확인한다.
        if (other.gameObject.tag == "Player")
        {
            moveButton.SetActive(true);
        }
    }
}
