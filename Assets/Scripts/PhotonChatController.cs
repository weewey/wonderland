using System.Collections.Generic;
using System.Threading.Tasks;
using Authentication;
using Controllers;
using ExitGames.Client.Photon;
using Opsive.Shared.Events;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using AuthenticationValues = Photon.Chat.AuthenticationValues;

public class PhotonChatController : MonoBehaviour, IChatClientListener
{
    [SerializeField] public Transform chatPrefab;
    [SerializeField] public TMP_InputField chatInput;
    [SerializeField] public string roomChannel;
    private ChatClient _chatClient;
    private HttpClient _httpClient;
    private Dictionary<string, GameObject> _userCharacters = new Dictionary<string, GameObject>();

    private void Awake()
    {
        Debug.Log("chat awake");
        EventHandler.RegisterEvent<Player, GameObject>("OnPlayerEnteredRoom", OnPlayerEnteredRoom);
        EventHandler.RegisterEvent<Player, GameObject>("OnPlayerLeftRoom", OnPlayerLeftRoom);
        chatInput.onEndEdit.AddListener(PublishMessage);
        _httpClient = new HttpClient(new JsonSerializationOption());
    }

    void Start()
    {
        Debug.Log("chat start");
        _chatClient = new ChatClient(this);
        ConnectToPhotonChat();
    }

    private void GetAllExistingPlayers()
    {
        var photonViews = FindObjectsOfType<PhotonView>();
        foreach (var view in photonViews)
        {
            if (view != null)
            {
                if (view.Controller != null)
                {
                    Debug.Log($"Get player controller {view.Controller.UserId}");
                }

                if (view.Owner != null)
                {
                    Debug.Log($"Get player owner {view.Owner.UserId}");
                }
            }
        }
    }

    private void SubscribeToRoomChannel()
    {
        _chatClient.Subscribe(new[] {roomChannel});
    }

    private void Update()
    {
        _chatClient.Service();
    }

    private void ConnectToPhotonChat()
    {
        _chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
            "1.0",
            new AuthenticationValues(PhotonNetwork.LocalPlayer.UserId));
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnDisconnected()
    {
        Debug.Log("You have been disconnected from Photon Chat");
    }

    public void OnConnected()
    {
        Debug.Log("You have been connected to Photon Chat");
        SubscribeToRoomChannel();
        _chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    private void OnPlayerEnteredRoom(Player player, GameObject character)
    {
        Debug.Log($"userId {player.UserId} entered");

        if (player.UserId == null) return;

        if (_userCharacters.ContainsKey(player.UserId))
        {
            _userCharacters[player.UserId] = character;
        }
        else
        {
            Debug.Log($"Adding user id {player.UserId}");
            _userCharacters.Add(player.UserId, character);
        }
    }

    private void OnPlayerLeftRoom(Player player, GameObject character)
    {
        _userCharacters.Remove(player.UserId);
    }

    public void SendDirectMessage(string recipient, string message)
    {
        _chatClient.SendPrivateMessage(recipient, message);
    }

    public void PublishMessage(string message)
    {
        var character = _userCharacters[PhotonNetwork.LocalPlayer.UserId].transform;
        if (!character) return;

        List<string> keys = new List<string>(_userCharacters.Keys);
        for (int j = 0; j < keys.Count; j++)
        {
            Debug.Log($"userid: {j} {keys[j]}");
        }

        _chatClient.PublishMessage(roomChannel, message);
        ChatBubble.Create(chatPrefab,
            character,
            new Vector3(0, 2, 0),
            message);
        chatInput.text = "";

        if (MessageHelper.IsPrivateMessage(message))
        {
            Debug.Log("private message");
             IncrementSocial();
        }
    }

    private async Task IncrementSocial()
    {
        string pubKey = SocialIndexController.GetCharacterPlayerInfo(PlayFabAuthService.Instance.GetWalletAddress())
            .CharAddr;
        string result = await _httpClient.Post("https://underland-wonderland.herokuapp.com/increment-social",
            new IncrementSocialRequest(pubKey, 5));
        Debug.Log(result);
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            if (senders[i] == PhotonNetwork.LocalPlayer.UserId) continue;


            var character = _userCharacters[senders[i]];
            ChatBubble.Create(chatPrefab,
                character.transform,
                new Vector3(0, 2, 0),
                messages[i].ToString());
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log($"Subscribed to {channels}. {results}");
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnUserSubscribed(string channel, string user)
    {
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }

    private void OnApplicationQuit()
    {
        if (_chatClient != null)
        {
            _chatClient.Disconnect();
        }
    }
}