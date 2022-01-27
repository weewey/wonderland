using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;

public class Lobby : MonoBehaviourPunCallbacks
{
    private Boolean _isConnecting;
    private Label _lobbyStatusLabel;
    protected int MaxPlayerCount = 8;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        _lobbyStatusLabel = rootVisualElement.Q<Label>("status");
        Connect();
    }

    public void Connect()
    {
        if (_isConnecting)
        {
            return;
        }

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
            _lobbyStatusLabel.text = "Joining an existing room.";
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            _lobbyStatusLabel.text = "Connecting to Photon Network";
        }

        _isConnecting = true;
    }

    public override void OnConnectedToMaster()
    {
        if (_isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            _lobbyStatusLabel.text = "Joining an existing room.";
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte) MaxPlayerCount;
        roomOptions.PublishUserId = true;
        PhotonNetwork.CreateRoom(null, roomOptions);
        _lobbyStatusLabel.text = "No rooms available. Creating a room.";
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(Constants.ConcertScene);
        _lobbyStatusLabel.text = "Connected to a room. Loading the level.";
    }
}