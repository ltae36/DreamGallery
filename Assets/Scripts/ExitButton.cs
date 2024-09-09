using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviourPun
{
    void Start()
    {

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
}
