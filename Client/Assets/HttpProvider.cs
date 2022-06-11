using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using BestHTTP;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using Models;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// Singleton for operations related to the player support system.
/// </summary>
public class HttpProvider : MonoBehaviour
{
    /// <summary>
    /// Singleton instance.
    /// </summary>
    public static HttpProvider Instance { get; private set; }

    public delegate void MessageReceiveDelegate(Message message);

    /// <summary>
    /// Even which is invoke when a message is received.
    /// </summary>
    public event MessageReceiveDelegate OnMessageReceive;

    private Guid? playerTokenId;
    private HubConnection chatHub;

    private void Start()
    {
        if (Instance == null) {
            Instance = this;
        } else if (Instance == this){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        chatHub = new HubConnection(new Uri("https://localhost:7058/ChatHub"), new JsonProtocol(new LitJsonEncoder()));
        chatHub.StartConnect();
    }

    /// <summary>
    /// Register a player.
    /// </summary>
    /// <param name="nickname">Player nickname.</param>
    /// <returns>If the registration was successful, return success RegistrationResponse with token,
    /// else return error RegistrationResponse with error message.</returns>
    public async Task<RegistrationResponse> RegisterPlayer(string nickname)
    {
        var request =
            new HTTPRequest(
                new Uri($"https://localhost:7058/Player/Register?deviceId={GetDeviceMacId()}&nickname={nickname}"));
        var response = await request.Send().GetHTTPResponseAsync();
        if (response.DataAsText is null) {
            return new RegistrationResponse() {
                Type = RegistrationResponse.ResponseType.Error,
                ErrorMessage = "Server is not available",
            };
        }
        var registrationResponse = JsonConvert.DeserializeObject<RegistrationResponse>(response.DataAsText);
        if (registrationResponse.Type == RegistrationResponse.ResponseType.Success) {
            playerTokenId = registrationResponse.AccessToken;
        }
        return registrationResponse;
    }

    /// <summary>
    /// Login a player.
    /// </summary>
    /// <param name="nickname">Player nickname.</param>
    /// <returns>If the login was successful, return success LoginResponse with token,
    /// else return error LoginResponse with error message.</returns>
    public async Task<LoginResponse> LoginPlayer(string nickname)
    {
        var request =
            new HTTPRequest(
                new Uri($"https://localhost:7058/Player/Login?deviceId={GetDeviceMacId()}&nickname={nickname}"));
        var response = await request.Send().GetHTTPResponseAsync();
        if (response.DataAsText is null) {
            return new LoginResponse() {
                Type = LoginResponse.ResponseType.Error,
                ErrorMessage = "Server is not available",
            };
        }
        var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response.DataAsText);
        if (loginResponse.Type == LoginResponse.ResponseType.Succes) {
            playerTokenId = loginResponse.AccessToken;
        }
        return loginResponse;
    }

    /// <summary>
    /// Subscribe to receive messages.
    /// </summary>
    public void SubscribeToReceiveMessages()
    {
        if (!IsAuthorize()) { return; }
        chatHub.On($"ReceiveMessage-{playerTokenId}",
            (string messageJson) => OnMessageReceive?.Invoke(JsonConvert.DeserializeObject<Message>(messageJson)));
    }

    /// <summary>
    /// Unsubscribe from receiving messages.
    /// </summary>
    public void UnsubscribeToReceiveMessages()
    {
        if (!IsAuthorize()) { return; }
        chatHub.Remove($"ReceiveMessage-{playerTokenId}");
    }

    /// <summary>
    /// Request for get past messages.
    /// </summary>
    /// <returns>List of messages sorted from oldest to newest.</returns>
    public async Task<Message[]> GetChatHistory()
    {
        if (!IsAuthorize()) { return Array.Empty<Message>(); }
        var request =
            new HTTPRequest(
                new Uri($"https://localhost:7058/Chat/ChatHistory?chatId={playerTokenId.Value}"));
        var response = await request.Send().GetHTTPResponseAsync();
        if (response is null) {
            Debug.LogError("Server is not available");
            return Array.Empty<Message>();
        }
        var messages = JsonConvert.DeserializeObject<Message[]>(response.DataAsText);
        return messages;
    }

    /// <summary>
    /// Send message to support.
    /// </summary>
    /// <param name="message">Text of message.</param>
    public new void SendMessage(string message)
    {
        if (!IsAuthorize()) { return; }
        chatHub.Send("SendToChatFromPlayer", playerTokenId.Value.ToString(), message);
    }

    /// <summary>
    /// Request for mark support messages as read.
    /// </summary>
    public void MarkMessageAsRead()
    {
        if (!IsAuthorize()) { return; }
        chatHub.Send("MarkOperatorMessagesAsRead", playerTokenId.Value.ToString());
    }

    /// <summary>
    /// Request for get the number of unread messages.
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetUnreadMessageCount()
    {
        if (!IsAuthorize()) { return 0; }
        var request =
            new HTTPRequest(
                new Uri($"https://localhost:7058/Chat/NumOfUnreadOperatorMessages?chatId={playerTokenId.Value}"));
        var response = await request.Send().GetHTTPResponseAsync();
        if (response is null) {
            Debug.LogError("Server is not available");
            return 0;
        }
        return Convert.ToInt32(response.DataAsText);
    }

    private string GetDeviceMacId() {
        var networkInterface = NetworkInterface.GetAllNetworkInterfaces()
            .FirstOrDefault(q => q.OperationalStatus == OperationalStatus.Up);
        if (networkInterface == null) {
            return string.Empty;
        }
        return BitConverter.ToString(networkInterface.GetPhysicalAddress().GetAddressBytes());
    }

    private bool IsAuthorize(bool logToDebug = true)
    {
        var authorized = playerTokenId is not null;
        if (logToDebug) {
            if (!authorized) {
                Debug.LogError("Server is not available");
            }
        }
        return authorized;
    }
}
