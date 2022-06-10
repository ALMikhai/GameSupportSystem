using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatMenu : MonoBehaviour
{
    private TMP_InputField messageField;
    private TMP_Text messages;

    async void Start()
    {
        messageField = GetComponentInChildren<TMP_InputField>();
        messages = transform.Find("Messages").GetComponent<TMP_Text>();
        HttpProvider.Instance.SubscribeToReceiveMessages();
        HttpProvider.Instance.OnMessageReceive += PrintMessage;
        var chatHistory = await HttpProvider.Instance.GetChatHistory();
        foreach (var message in chatHistory) {
            PrintMessage(message);
        }
    }

    public void OnSendMessageButtonClick()
    {
        HttpProvider.Instance.SendMessage(messageField.text);
    }

    private void PrintMessage(Message message)
    {
        messages.text += $"\n{message.Name}: {message.Text}";
    }
}
