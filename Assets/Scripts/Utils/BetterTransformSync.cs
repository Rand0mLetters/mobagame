using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterTransformSync : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool syncPosition;
    public bool syncRotation;
    bool isMine;

    // Update is called once per frame
    void Update()
    {
        isMine = photonView.IsMine;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting && isMine) {
            if (syncPosition) {
                stream.SendNext(transform.position.x);
                stream.SendNext(transform.position.y);
                stream.SendNext(transform.position.z);
            }
            if (syncRotation) {
                stream.SendNext(transform.eulerAngles.x);
                stream.SendNext(transform.eulerAngles.y);
                stream.SendNext(transform.eulerAngles.z);
            }
        } else if(!isMine && stream.IsReading){
            if(syncPosition) {
                Vector3 newPos = new();
                newPos.x = (float) stream.ReceiveNext();
                newPos.y = (float) stream.ReceiveNext();
                newPos.z = (float) stream.ReceiveNext();
                transform.position = newPos;
            }
            if (syncRotation) {
                Vector3 newRot = new();
                newRot.x = (float) stream.ReceiveNext();
                newRot.y = (float) stream.ReceiveNext();
                newRot.z = (float) stream.ReceiveNext();
                transform.eulerAngles = newRot;
            }
        }
    }
}
