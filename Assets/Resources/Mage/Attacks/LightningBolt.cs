using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Mage/Lightning Bolt", fileName = "[MAGE][ATTACK] Lightning Bolt")]
public class LightningBolt : Ability
{
    public GameObject lightingBolt;
    public GameObject aimIndicator;
    public float time = 3f;
    GameObjectPooler p;
    GameObject aim;
    System.Action<Ability> callback;

    public override void Activate(GameObjectPooler pooler, System.Action<Ability> cb) {
        p = pooler;
        callback = cb;
        aim = p.Instantiate(aimIndicator);
        aim.GetComponent<FollowMouse>().BindCallback(OnAimFinished);
    }

    public void OnAimFinished(RaycastHit hit) {
        p.Destroy(aim);
        PhotonNetwork.Instantiate(lightingBolt.name, hit.point, Quaternion.identity);
        callback(this);
    }
}
