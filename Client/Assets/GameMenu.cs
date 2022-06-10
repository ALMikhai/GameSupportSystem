using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    private TMP_Text unreadMessageCountText;

    async void Start()
    {
        unreadMessageCountText = transform.Find("UnreadMessageCount").GetComponent<TMP_Text>();
        var unreadMessage = await HttpProvider.Instance.GetUnreadMessageCount();
        if (unreadMessage != 0) {
            unreadMessageCountText.text = $"{unreadMessage} unread message";
        }
    }

    public void OnChatButtonClick()
    {
        SceneManager.LoadScene("ChatScene", LoadSceneMode.Single);
    }
}
