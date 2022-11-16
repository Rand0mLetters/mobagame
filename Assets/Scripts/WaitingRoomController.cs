using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Collections;
using UnityEngine.AddressableAssets;

public class WaitingRoomController : MonoBehaviourPunCallbacks
{
    public AssetReference sceneIndex;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.LocalPlayer.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            LoadNextLevel();
        }
    }

    public void LoadNextLevel() {
        PhotonNetwork.LoadLevel(sceneIndex);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(false);
    }
}
