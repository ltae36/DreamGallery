using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    void Start()
    {
        
    }

    public void ToCon()
    {
        SceneManager.LoadScene("ConnectScene");
    }
}
