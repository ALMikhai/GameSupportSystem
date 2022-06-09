using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using BestHTTP;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using Newtonsoft.Json;
using UnityEngine;

public class HttpProvider : MonoBehaviour
{
    public static HttpProvider Instance { get; private set; }

    public delegate void MessageReceiveDelegate(string sourceName, string message);

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
        if (registrationResponse.Type == RegistrationResponse.ResponseType.Succes) {
            playerTokenId = registrationResponse.AccessToken;
        }
        return registrationResponse;
    }

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

    public void SubscribeToReceiveMessages()
    {
        if (!IsAuthorize()) { return; }
        chatHub.On($"ReceiveMessage-{playerTokenId}",
            (string sourceName, string message) => OnMessageReceive?.Invoke(sourceName, message));
    }
    
    public void UnsubscribeToReceiveMessages()
    {
        if (!IsAuthorize()) { return; }
        chatHub.Remove($"ReceiveMessage-{playerTokenId}");
    }
    
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

    public new void SendMessage(string message)
    {
        if (!IsAuthorize()) { return; }
        chatHub.Send("SendToChatFromPlayer", playerTokenId.Value.ToString(), message);
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

public class Message
{
    public enum SourceType {
        Player,
        Operator,
    }

    public int Id { get; set; }
    public Guid ChatId { get; set; }
    public SourceType Type { get; set; }
    public string Name { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public bool IsReaded { get; set; } = false;
    public string Text { get; set; } = string.Empty;
}

public class RegistrationResponse
{
    public enum ResponseType
    {
        Error,
        Succes,
    }

    public ResponseType Type { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public Guid AccessToken { get; set; }
}

public class LoginResponse
{
    public enum ResponseType
    {
        Error,
        Succes,
    }

    public ResponseType Type { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public Guid AccessToken { get; set; }
}
