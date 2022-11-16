using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesSynchronizer : MonoBehaviourPunCallbacks {
    public PlayerMatchData matchData;
    public static PlayerStatesSynchronizer instance;

    private void Awake() {
        instance = this;
        matchData = new();
        matchData.playerId = PhotonNetwork.LocalPlayer.UserId;
    }
}
