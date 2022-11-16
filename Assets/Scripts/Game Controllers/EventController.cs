using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class EventController : MonoBehaviourPunCallbacks
{
    public static EventController instance;
    public float rejoinTime = 3f;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public override void OnDisconnected(DisconnectCause cause) {
        if (AlertController.instance != null) AlertController.instance.Alert("DIsCoNnECteD");
        Debug.Log(cause.ToString());
        AttemptRejoin();
    }

    public override void OnErrorInfo(ErrorInfo errorInfo) {
        if (AlertController.instance != null) AlertController.instance.Alert("ErorRe");
        Debug.Log(errorInfo.Info.ToString());
        AttemptRejoin();
    }

    private void AttemptRejoin() {
        bool attempt = PhotonNetwork.ReconnectAndRejoin();
        Debug.Log("Rejoin Attempted at " + Time.time + " with result " + attempt);
        if (attempt) {
            CancelInvoke("AttemptRejoin");
        } else Invoke("AttemptRejoin", rejoinTime);
    }

    private void OnApplicationPause(bool pause) {
        if (pause) {
            Debug.Log("aaaaaaa");
        }
    }
}
