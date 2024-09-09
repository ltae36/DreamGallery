using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneScript : MonoBehaviour
{

    public int paintedCount = 0;
    public int question1Count = 100;
    public int question2Count = 200;
    public GameObject ChatUI;
    public Text question;
    public GameObject exitChat;

    public float Timer = 10;


    bool question1 = true;
    bool question2 = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
        if (paintedCount >= question1Count)
        {
            if (question1)
            {
                question1 = !question1;
                ChatUI.SetActive(true);
                question.text = "어떤 그림을 그리시나요? 얼마나 그리셨나요?";
                StartCoroutine(ButtonOn());
            }
        }
        if (paintedCount >= question2Count)
        {
            if (question2)
            {
                question2 = !question2;
                ChatUI.SetActive(true);
                question.text = "그림의 의도가 있으신가요?";
                StartCoroutine(ButtonOn());
            }
        }
    }

    IEnumerator ButtonOn()
    {
        yield return new WaitForSeconds(5);

        exitChat.SetActive(true);
    }
}
