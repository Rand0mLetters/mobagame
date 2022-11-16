using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChampionDemoController : MonoBehaviourPunCallbacks
{
    public TMPro.TextMeshProUGUI championNameDisplay;
    public CharacterData[] datas;
    public CharacterData selectedChampion;
    GameObject spawnedChampion;
    public GameObject botPrefab;
    public static ChampionDemoController instance;

    private void Awake()
    {
        instance = this;
        selectedChampion = datas[0];
    }

    IEnumerator Start()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected) yield return null;
        PhotonNetwork.OfflineMode = true;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.CreateRoom("Demo");
    }

    public override void OnJoinedRoom()
    {
        DemoChampion(selectedChampion);
    }

    public void DemoChampion(CharacterData newChampion)
    {
        selectedChampion = newChampion;
        championNameDisplay.text = selectedChampion.characterName;
        if(spawnedChampion) PhotonNetwork.Destroy(spawnedChampion);
        spawnedChampion = PhotonNetwork.Instantiate(selectedChampion.characterName + "/" + selectedChampion.characterName,Vector3.zero, Quaternion.identity);
        CameraFollowLocalPlayer.instance.FindTarget();
    }

    public void SpawnBot(Vector3 position, Quaternion identity)
    {
        PhotonNetwork.Instantiate("Bots/" + botPrefab.name, position, identity);
    }
}