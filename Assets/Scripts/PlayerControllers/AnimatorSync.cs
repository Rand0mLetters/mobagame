using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimatorSync : MonoBehaviourPunCallbacks, IPunObservable
{
    public Animator anim;
    public bool isMine = false;
    public string lastTrigger;


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        isMine = photonView.Owner.UserId == PhotonNetwork.LocalPlayer.UserId;
        if (stream.IsWriting && isMine)
        {
            stream.SendNext(lastTrigger);
        }else if(!stream.IsWriting && !isMine)
        {
            SetTrigger((string)stream.ReceiveNext());
        }
    }

    public void SetTrigger(string trigger)
    {
        if (trigger.Length == 0) return;
        ResetAllTriggers();
        anim.SetTrigger(trigger);
        lastTrigger = trigger;
    }

    private void ResetAllTriggers(){
        // anim is null (lol)
        foreach (AnimatorControllerParameter param in anim.parameters){
            if (param.type == AnimatorControllerParameterType.Trigger){
                anim.ResetTrigger(param.name);
            }
        }
    }
}
