using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AddressableAssets;

public class TeamController : MonoBehaviourPunCallbacks
{
    public static TeamController instance;
    public AssetReference nextScene;
    public List<string> xanaduTeam;
    public List<string> arcadiaTeam;
    bool isMaster;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            isMaster = true;
            StartCoroutine("DistributeTeams");
        }
        else
        {
            isMaster = false;
        }
    }

    public IEnumerator DistributeTeams()
    {
        if (!isMaster) yield break;
        float startTime = Time.time;
        bool canStart = false;
        while(Time.time - startTime < 30 && !canStart) {
            if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers) {
                canStart = true;
            }
            yield return new WaitForEndOfFrame();
        }
        if (!canStart) {
            // return to menu
        }
        for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            byte team = 1;
            if (i < PhotonNetwork.CurrentRoom.MaxPlayers / 2) team = 0;
            photonView.RPC("AssignPlayerToTeam", RpcTarget.All, new object[] { PhotonNetwork.PlayerList[i].UserId, team });
        }
        yield return new WaitForSeconds(0.1f);
        PhotonNetwork.LoadLevel(nextScene);
    }

    [PunRPC]
    public void AssignPlayerToTeam(object[] objs)
    {
        if((byte) objs[1] == 0)
        {
            xanaduTeam.Add((string) objs[0]);
        }
        else
        {
            arcadiaTeam.Add((string) objs[0]);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject go in gos) {
            if(go.GetComponent<PhotonView>().Owner.UserId == otherPlayer.UserId) Destroy(go);
        }
    }

    public override void OnMasterClientSwitched(Player newMaster)
    {
        if (newMaster.UserId == PhotonNetwork.LocalPlayer.UserId)
        {
            isMaster = true;
        }
    }

    public bool PlayerOnArcadia(string playerNumber)
    {
        return arcadiaTeam.Contains(playerNumber);
    }

    public bool AreTeammates(string player1, string player2)
    {
        return PlayerOnArcadia(player1) == PlayerOnArcadia(player2);
    }

    public int ReturnEnemyPlayer(string playerNumber)
    {
        if (PlayerOnArcadia(playerNumber)) return GetPlayerWithID(xanaduTeam[0]).ActorNumber;
        return GetPlayerWithID(arcadiaTeam[0]).ActorNumber;
    }

    public Player GetPlayerWithID(string id) {
        foreach(Player p in PhotonNetwork.PlayerList) {
            if (p.UserId == id) return p;
        }
        return PhotonNetwork.LocalPlayer;
    }

    public Player GetPlayerWithActorNumber(int num) {
        foreach (Player p in PhotonNetwork.PlayerList) {
            if (p.ActorNumber == num) return p;
        }
        return PhotonNetwork.LocalPlayer;
    }
}
