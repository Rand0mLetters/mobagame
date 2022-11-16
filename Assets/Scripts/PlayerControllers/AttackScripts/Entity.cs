using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Entity_Types {
    CREEP = 25,
    PLAYER = 100,
    TOWER = 250
}

[RequireComponent(typeof(Damageable))]
public class Entity : MonoBehaviourPunCallbacks, IPointerClickHandler
{
    public Entity_Types type;
    public bool isTeammate;
    public bool isMine;
    public bool isDead;
    public bool isArcadian;
    public bool hasGivenReward;
    public Damageable damageable;
    public PlayerMatchData playerMatchData;

    IEnumerator Start()
    {
        damageable = GetComponent<Damageable>();
        yield return new WaitForSeconds(0.15f);
        if (type == Entity_Types.PLAYER) playerMatchData = PlayerStatesSynchronizer.instance.matchData;
        InvokeRepeating("CheckTeam", 0, 5);
    }

    void Update() {
        isMine = photonView.IsMine;
    }

    void CheckTeam() {
        isArcadian = TeamController.instance.PlayerOnArcadia(TeamController.instance.GetPlayerWithActorNumber(photonView.OwnerActorNr).UserId);
        isTeammate = photonView.Owner != null && TeamController.instance.AreTeammates(photonView.Owner.UserId, PhotonNetwork.LocalPlayer.UserId);
    }

    public void OnPointerClick(PointerEventData eventData) {
        // display data
    }
}
