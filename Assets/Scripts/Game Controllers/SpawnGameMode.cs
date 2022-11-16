using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnGameMode : MonoBehaviourPunCallbacks
{
    public GameModeData[] modes;
    public GameModeData gameMode;

    void Awake()
    {
        string gmn = (string) PhotonNetwork.CurrentRoom.CustomProperties["gmn"];
        foreach(GameModeData mode in modes)
        {
            if(mode.gameModeName == gmn)
            {
                gameMode = mode;
                break;
            }
        }
        if(gameMode != null && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.Instantiate(gameMode.modeManager.name, Vector3.zero, Quaternion.identity);
        }
    }
}
