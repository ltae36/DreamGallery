using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.SendRate = 60;

        PhotonNetwork.SerializationRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
