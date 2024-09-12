using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingModeTrigger : MonoBehaviour
{
    // 플레이어의 앞에 놓인 캔버스만 선택할 수 있다.
    ClickEaselAction cea;
    PaintingMode pm;
    GameObject mgr;
    //PaintingModeMgr pmm;

    void Start()
    {
        cea = GetComponentInParent<ClickEaselAction>();
        pm = GetComponentInParent<PaintingMode>();
        mgr = GameObject.Find("manager");

        cea.enabled = false;
        pm.enabled = false;
        //pmm = mgr.GetComponent<PaintingModeMgr>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어가 들어오면 캔버스를 선택 가능
        if (other.gameObject.tag == "Player")
        {
            cea.enabled = true;
            pm.enabled = true;
            //pmm.playerCheck = true;
        }
    }
}
