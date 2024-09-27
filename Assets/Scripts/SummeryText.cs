using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummeryText : MonoBehaviour
{
    public static SummeryText instance;
    public string summeryText;

    ChatSummary chatSummary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        chatSummary = GetComponent<ChatSummary>();

        // PlayerPrefs에서 이전에 저장된 summeryText 값 불러오기
        if (PlayerPrefs.HasKey("SummeryText"))
        {
            summeryText = PlayerPrefs.GetString("SummeryText");
        }
    }

    void Update()
    {
        if (chatSummary != null && chatSummary.result_text != null)
        {
            summeryText = chatSummary.result_text.text;

            // summeryText 값을 PlayerPrefs에 저장
            PlayerPrefs.SetString("SummeryText", summeryText);
        }
    }

    private void OnApplicationQuit()
    {
        // 게임 종료 시 PlayerPrefs에 summeryText 저장
        PlayerPrefs.SetString("SummeryText", summeryText);
    }
}
