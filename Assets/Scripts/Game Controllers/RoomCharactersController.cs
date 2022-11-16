using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomCharactersController : MonoBehaviourPunCallbacks
{
    public Dictionary<string, CharacterData> characters;
    public static RoomCharactersController instance;
    public CharacterData[] allCharacters;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        characters = new Dictionary<string, CharacterData>();
    }

    public void SelectCharacterForLocalPlayer(int id)
    {
        photonView.RPC("SelectCharacterForPlayer", RpcTarget.All, PhotonNetwork.LocalPlayer.UserId, id);
    }

    [PunRPC]
    public void SelectCharacterForPlayer(string playerNumber, int characterId)
    {
        if (!characters.ContainsKey(playerNumber))
        {
            characters.Add(playerNumber, allCharacters[characterId]);
        }
        else
        {
            characters[playerNumber] = allCharacters[characterId];
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

    public CharacterData GetCharacterOfPlayer(string playerNumber)
    {
        if (characters.ContainsKey(playerNumber))
        {
            return characters[playerNumber];
        }
        else
        {
            return allCharacters[0];
        }
    }
}
