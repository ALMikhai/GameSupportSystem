using Models;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Chat menu.
/// </summary>
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

    /// <summary>
    /// Send message.
    /// </summary>
    public void OnSendMessageButtonClick()
    {
        HttpProvider.Instance.SendMessage(messageField.text);
    }

    /// <summary>
    /// Set active game scene.
    /// </summary>
    public void OnBackButtonClick()
    {
        HttpProvider.Instance.UnsubscribeToReceiveMessages();
        HttpProvider.Instance.OnMessageReceive -= PrintMessage;
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    /// <summary>
    /// Send mark massage as read request.
    /// </summary>
    public void OnMarkAsReadClick()
    {
        HttpProvider.Instance.MarkMessageAsRead();
    }

    private void PrintMessage(Message message)
    {
        messages.text += $"\n{message.Name}: {message.Text}";
    }
}
