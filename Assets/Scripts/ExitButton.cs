using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviourPun
{
    public GameObject endGameScreen; // 게임 종료 창

    void Start()
    {
        endGameScreen.SetActive(false);
    }

    private void Update()
    {
        // ESC를 누르면 종료창이 뜬다
        if (Input.GetKeyDown(KeyCode.Escape)) endGameScreen.SetActive(true);
    }

    public void ClickExit()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LoadLevel("ConnectScene");
        }
        else 
        {
            SceneManager.LoadScene("ConnectScene");
        }
    }

    // 게임 종료
    public void EndGame() 
    {
        Application.Quit();
    }

    public void CloseEndGame() 
    {
        endGameScreen.SetActive(false);
    }
}
