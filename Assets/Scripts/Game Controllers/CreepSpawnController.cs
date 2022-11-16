using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class Lane {
    public Transform[] waypoints;
    public Transform[] arcadiaCreepSpawns;
    public Transform[] xanaduCreepSpawns;
}

public class CreepSpawnController : MonoBehaviourPun
{
    public GameObject creep;
    public Lane[] lanes;
    public static CreepSpawnController instance;
    int newOwner;

    private void Awake() {
        instance = this;
    }

    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient) {
            InvokeRepeating("SpawnXanaduCreeps", 0, 30);
            InvokeRepeating("SpawnArcadiaCreeps", 0, 30);
            newOwner = TeamController.instance.ReturnEnemyPlayer(PhotonNetwork.LocalPlayer.UserId);
        }
    }

    void SpawnArcadiaCreeps()
    {
        for(int i = 0; i < lanes.Length; i++) {
            foreach(Transform t in lanes[i].arcadiaCreepSpawns) {
                GameObject go = PhotonNetwork.InstantiateRoomObject("Bots/" + creep.name, t.position, t.rotation);
                go.GetComponent<PhotonView>().TransferOwnership(newOwner);
                // set creep properties
                CreepBehaviourController controller = go.GetComponent<CreepBehaviourController>();
                controller.SetLane((LANE) i);
            }
        }
    }

    void SpawnXanaduCreeps() {
        for(int i = 0; i < lanes.Length; i++){ 
            foreach(Transform spawn in lanes[i].xanaduCreepSpawns) {
                GameObject go = PhotonNetwork.InstantiateRoomObject("Bots/" + creep.name, spawn.position, spawn.rotation);
                // set creep properties.
                CreepBehaviourController controller = go.GetComponent<CreepBehaviourController>();
                controller.SetLane((LANE) i);
            }
        }
    }
}
