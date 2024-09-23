using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneMgr : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            SceneManager.LoadScene("Connection");
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            SceneManager.LoadScene("ConnectScene");
        }
    }

    public void StartScene() 
    {
        SceneManager.LoadScene("ConnectScene");
    }
}
