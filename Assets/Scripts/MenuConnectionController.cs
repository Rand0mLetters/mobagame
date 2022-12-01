using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine;
using UnityEngine.AddressableAssets;
using TMPro;

public class MenuConnectionController : MonoBehaviourPunCallbacks, IConnectionCallbacks
{
    public string gameVersion = "v0.0.1b";
    public bool canPlay = false;
    public Button playButton;
    public GameModeData[] gameModes;
    public int sI;
    
    void Awake()
    {
        Application.runInBackground = true;
        if(GameTime.exists) GameTime.Kill();
    }

    void Start()
    {
        PhotonNetwork.OfflineMode = false;
        if (!PhotonNetwork.IsConnected)
        {
            playButton.interactable = false;
            PhotonNetwork.AutomaticallySyncScene = true;
            try
            {
                PhotonNetwork.ConnectUsingSettings();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
            PhotonNetwork.GameVersion = gameVersion;
        }
        else
        {
            canPlay = true;
            playButton.interactable = true;
            PhotonNetwork.LocalPlayer.SetUserId(PlayerDataController.instance.data.PlayFabId);
        }
    }

    public void Play()
    {
        if (canPlay) {
            PhotonNetwork.LocalPlayer.NickName = PlayerDataController.instance.username;
            try {
                Hashtable expectedProperties = new() {
                    {"gmn", gameModes[sI].gameModeName }
                };
                PhotonNetwork.JoinRandomRoom(expectedProperties, gameModes[sI].numberOfPlayers);
            }catch (System.Exception) {
                PhotonNetwork.Disconnect();
                PhotonNetwork.ConnectUsingSettings();
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        canPlay = true;
        playButton.interactable = true;
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PlayerDataController.instance == null) { PhotonNetwork.LocalPlayer.SetUserId(Random.Range(0, 1000).ToString()); return; }
        PhotonNetwork.KeepAliveInBackground = int.MaxValue;
        PhotonNetwork.LocalPlayer.NickName = PlayerDataController.instance.data.PlayFabId;
        PhotonNetwork.LocalPlayer.SetUserId(PlayerDataController.instance.data.PlayFabId);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new();
        roomOptions.MaxPlayers = gameModes[sI].numberOfPlayers;
        roomOptions.PlayerTtl = int.MaxValue;
        roomOptions.PublishUserId = true;
        roomOptions.CustomRoomProperties = new Hashtable
        {
            {"gmn", gameModes[sI].gameModeName },
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "gmn" };
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnCreatedRoom()
    {
        playButton.interactable = false;
        playButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Finding match";
    }
}
