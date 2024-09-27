using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSummery : MonoBehaviour
{
    string diary;
    GameObject summery;
    SummeryText summeryText;
    public TMP_Text diaryText;

    void Start()
    {
        summery = GameObject.Find("SummeryTextLog");
        summeryText = summery.GetComponent<SummeryText>();
    }

    void Update()
    {
        diaryText.text = summeryText.summeryText;
    }
}
