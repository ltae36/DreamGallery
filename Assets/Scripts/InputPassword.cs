using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPassword : MonoBehaviour
{
    InputField inputPassword;
    public NetworkConnection net;

    void Start()
    {
        inputPassword = GetComponent<InputField>();
    }

    void Update()
    {
        net.enteredPassword = inputPassword.text;
    }
}
