using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HideObjectInFog : MonoBehaviourPunCallbacks
{
    public MonoBehaviour[] components;
    public GameObject[] gos;
    public int cols;

    IEnumerator Start() {
        if (!TeamController.instance) {
            Destroy(this);
            yield break;
        }
        yield return new WaitForSeconds(.1f);
        if (gameObject.CompareTag("Player") && TeamController.instance.AreTeammates(PhotonNetwork.LocalPlayer.UserId, photonView.Owner.UserId))
        {
            Destroy(this);
            SetComponents(true);
        }
        else
        {
            SetComponents(false);
        }
    }

    void SetComponents(bool state)
    {
        foreach(MonoBehaviour mb in components)
        {
            mb.enabled = state;
        }
        foreach(GameObject go in gos)
        {
            go.SetActive(state);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        try {
            PhotonView view = col.GetComponent<PhotonView>();
            if (!col.CompareTag("fow") && ((view && view.Owner.UserId != PhotonNetwork.LocalPlayer.UserId) || !view)) return;
            if (cols >= 0) SetComponents(true);
            cols += 1;
        }catch(System.Exception) { }
    }

    private void OnTriggerExit(Collider other)
    {
        PhotonView view = other.GetComponent<PhotonView>();
        if (!other.CompareTag("fow") && ((view && view.Owner.UserId != PhotonNetwork.LocalPlayer.UserId) || !view)) return;
        cols -= 1;
        if (cols <= 0)
        {
            cols = 0;
            SetComponents(false);
        }
    }
}
