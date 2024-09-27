using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSummery : SummeryText
{
    string diary;

    public TMP_Text diaryText;

    void Start()
    {
        PlayerPrefs.GetString("SummeryText", summeryText);
    }

    void Update()
    {
        diary = summeryText;
        diaryText.text = diary;
    }
}
