using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviourPunCallbacks
{
    public bool belongsToTeammate;

    void Start()
    {
        belongsToTeammate = TeamController.instance.AreTeammates(PhotonNetwork.LocalPlayer.UserId, photonView.Owner.UserId);
    }

    private void OnMouseOver() {
        if (belongsToTeammate) {

        } else {

        }
    }

    private void OnMouseExit() {
        
    }

    private void OnMouseDown() {
        // show stats
    }
}
