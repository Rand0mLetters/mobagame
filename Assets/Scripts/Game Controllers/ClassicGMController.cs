using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.AddressableAssets;

public class ClassicGMController : MonoBehaviourPunCallbacks
{
    public GameObject towerPrefab;
    public GameObject postGameData;
    public AssetReference sceneIndex;

    GameObject xanaduTower;
    bool gameOver = false;
    public static ClassicGMController instance;

    void Awake()
    {
        instance = this;
        Instantiate(postGameData);
        StartCoroutine("wake");
    }

    IEnumerator wake()
    {
        yield return new WaitForEndOfFrame();
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            // spawn towers
            Transform arcadiaSpawnPosition = TeamSpawnController.instance.GetTowerSpawnPosition(false);
            PhotonNetwork.InstantiateRoomObject(towerPrefab.name, arcadiaSpawnPosition.position, arcadiaSpawnPosition.rotation);

            Transform xanaduSpawnPosition = TeamSpawnController.instance.GetTowerSpawnPosition(true);
            xanaduTower = PhotonNetwork.InstantiateRoomObject(towerPrefab.name, xanaduSpawnPosition.position, xanaduSpawnPosition.rotation);
            xanaduTower.GetComponent<PhotonView>().TransferOwnership(TeamController.instance.ReturnEnemyPlayer(PhotonNetwork.LocalPlayer.UserId));
        }
    }

    public void RegisterTowerDestroyed(bool isCapital, bool arcadiaTower)
    {
        if(!gameOver) photonView.RPC("TowerDestroyed", RpcTarget.All, new bool[]{ isCapital, arcadiaTower});
        gameOver = true;
    }

    [PunRPC]
    public void TowerDestroyed(bool[] datum)
    {
        if (datum[0])
        {
            gameOver = true;
            if (datum[1])
            {
                // arcadia lose
                PostgameData.instance.xanaduScore = 1;
            }
            else
            {
                // arcadia win
                PostgameData.instance.arcadiaScore = 1;
            }
            bool onArcadia = TeamController.instance.PlayerOnArcadia(PhotonNetwork.LocalPlayer.UserId);
            PostgameData.instance.won = onArcadia == !datum[1];
            PostgameData.instance.playerOnArcadia = onArcadia;
            PhotonNetwork.LeaveRoom();
            sceneIndex.LoadSceneAsync();
        }
        else
        {
            // idk man
        }
    }
}
