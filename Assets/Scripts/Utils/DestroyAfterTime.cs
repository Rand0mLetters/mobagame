using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DestroyAfterTime : MonoBehaviourPunCallbacks{
    public float time;
    public bool isNetworked;

    private void Start() {
        Invoke("DestroyObject", time);
    }

    void DestroyObject() {
        if(isNetworked && photonView.IsMine) {
            PhotonNetwork.Destroy(gameObject);
        }else if (!isNetworked) {
            Destroy(gameObject);
        }
    }
}