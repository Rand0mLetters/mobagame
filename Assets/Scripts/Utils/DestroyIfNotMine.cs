using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfNotMine : MonoBehaviourPunCallbacks
{
    public MonoBehaviour[] components;
    public GameObject[] gos;
    Entity myself;
    void Start()
    {
        myself = GetComponent<Entity>();
    }

    private void Update() {
        SetStuffInacive(myself.isMine);
    }

    void SetStuffInacive(bool active) {
        if (photonView.Owner.UserId != PhotonNetwork.LocalPlayer.UserId) {
            foreach (MonoBehaviour mb in components) {
                mb.enabled = active;
            }
            foreach (GameObject go in gos) {
                go.SetActive(active);
            }
        }
    }
}
