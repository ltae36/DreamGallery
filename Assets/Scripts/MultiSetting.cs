using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSetting : MonoBehaviour
{
    public GameObject settingWindow;
    public GameObject inputField;

    void Start()
    {
        settingWindow.SetActive(false);
        inputField.SetActive(false);
    }

    public void ClickBotton() 
    {
        settingWindow.SetActive(true);
    }

    public void EnterRoom() 
    {
        inputField.SetActive(true);
    }
}
