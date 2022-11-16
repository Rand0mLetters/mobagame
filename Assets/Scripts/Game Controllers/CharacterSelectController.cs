using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AddressableAssets;

public class CharacterSelectController : MonoBehaviourPunCallbacks
{
    bool isMaster = false;
    public int playersReady = 0;
    public int selectedCharacterId = -1;
    public AssetReference reference;
    bool isReady = false;

    void Awake()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            isMaster = true;
        }
    }

    public void SelectCharacter(int characterId)
    {
        RoomCharactersController.instance.SelectCharacterForLocalPlayer(characterId);
        selectedCharacterId = characterId;
    }

    public void ReadyLocalPlayer()
    {
        if(selectedCharacterId == -1)
        {
            Debug.Log("SELECT a CHARAcTeR");
            return;
        }
        if (!isReady)
        {
            isReady = true;
            photonView.RPC("ReadyPlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    public void ReadyPlayer()
    {
        playersReady++;
        if(isMaster && playersReady == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            PhotonNetwork.LoadLevel(reference);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(newMasterClient.UserId == PhotonNetwork.LocalPlayer.UserId)
        {
            isMaster = true;
        }
    }
}