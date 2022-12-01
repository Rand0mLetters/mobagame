using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TeamSpawnController : MonoBehaviourPunCallbacks
{
    public Transform[] arcadiaSpawns;
    public Transform[] xanaduSpawns;

    public Transform[] arcadiaTowerSpawns;
    public Transform[] xanaduTowerSpawns;

    public GameObject character;
    public GameObject tower;

    int newOwner;
    string playerPath;

    public static TeamSpawnController instance;

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;
        CharacterData playerChar = RoomCharactersController.instance.GetCharacterOfPlayer(PhotonNetwork.LocalPlayer.UserId);
        playerPath = playerChar.character.name + "/" + playerChar.character.name;
    }

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(Random.Range(0, .25f));
        SpawnPlayer(PhotonNetwork.LocalPlayer.UserId);
        if (PhotonNetwork.LocalPlayer.IsMasterClient) {
            newOwner = TeamController.instance.ReturnEnemyPlayer(PhotonNetwork.LocalPlayer.UserId);
            SpawnXanaduTowers();
            SpawnArcadiaTowers();
        }
    }

    public void SpawnPlayer(string playerNumber)
    {
        Transform spawnPoint;
        if (TeamController.instance.PlayerOnArcadia(playerNumber))
        {
            spawnPoint = GetSpawnPoint(arcadiaSpawns);
        }
        else
        {
            spawnPoint = GetSpawnPoint(xanaduSpawns);
        }
        PhotonNetwork.Instantiate(playerPath, spawnPoint.position, spawnPoint.rotation);
    }

    public void SpawnArcadiaTowers()
    {
        for (int i = 1; i < arcadiaTowerSpawns.Length; i++)
        {
            GameObject go = PhotonNetwork.InstantiateRoomObject(tower.name, arcadiaTowerSpawns[i].position, arcadiaTowerSpawns[i].rotation);
            go.GetComponent<PhotonView>().TransferOwnership(newOwner);
        }
    }

    public void SpawnXanaduTowers() {
        for (int i = 1; i < xanaduTowerSpawns.Length; i++) {
            PhotonNetwork.InstantiateRoomObject(tower.name, xanaduTowerSpawns[i].position, xanaduTowerSpawns[i].rotation);
        }
    }

    Transform GetSpawnPoint(Transform[] spawns)
    {
        foreach(Transform t in spawns)
        {
            if(Physics.OverlapSphere(t.position, 1).Length == 0)
            {
                return t;
            }
        }
        return spawns[Random.Range(0, spawns.Length)];
    }

    public Transform GetTowerSpawnPosition(bool forArcadia)
    {
        if (forArcadia)
        {
            return arcadiaTowerSpawns[0];
        }
        else
        {
            return xanaduTowerSpawns[0];
        }
    }
}
