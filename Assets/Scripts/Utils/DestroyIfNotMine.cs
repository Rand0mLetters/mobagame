using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfNotMine : MonoBehaviourPunCallbacks
{
    public MonoBehaviour[] components;
    public GameObject[] gos;
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (photonView.Owner.UserId != PhotonNetwork.LocalPlayer.UserId)
        {
            foreach(MonoBehaviour mb in components)
            {
                Destroy(mb);
            }
            foreach (GameObject go in gos)
            {
                Destroy(go);
            }
        }
        Destroy(this);
    }
}
