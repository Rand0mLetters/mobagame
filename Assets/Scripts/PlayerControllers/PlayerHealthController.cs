using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerHealthController : MonoBehaviourPunCallbacks, IPunObservable
{
    public static PlayerHealthController instance;
    public float health;
    public float healthRegen = 1f;
    public GameObject floatingText;
    public Slider healthBar;
    public Slider funnyDelayedSlider;
    float funnyDelayedSliderTarget;
    public TMPro.TextMeshProUGUI nameDisplay;
    public Animator anim;
    public NavMeshAgent agent;
    public Collider col;
    Entity myself;
    Vector3 pos;
    Quaternion rot;

    void Start()
    {
        health = PlayerBuffController.instance.health;
        PlayerStatesSynchronizer.instance.matchData.health = PlayerBuffController.instance.health;
        healthBar.value = 1;
        if (funnyDelayedSlider) {
            funnyDelayedSlider.value = 1;
            funnyDelayedSliderTarget = 1;
        }
        if(nameDisplay) nameDisplay.text = photonView.Owner.NickName;
        pos = transform.position;
        rot = transform.rotation;
        myself = GetComponent<Entity>();
    }

    private void Update()
    {
        //oh no
        UpdateDisplay();
        if (myself.isDead)
        {
            myself.isDead = true;
            if(anim) anim.SetTrigger("Die");
            if (agent) agent.enabled = false;
            if(col) col.enabled = false;
            healthBar.gameObject.SetActive(false);
            if(funnyDelayedSlider) funnyDelayedSlider.gameObject.SetActive(false);
        }
        if(funnyDelayedSlider && funnyDelayedSlider.value > funnyDelayedSliderTarget) {
            float newVal = funnyDelayedSlider.value - 25 * Time.deltaTime;
            funnyDelayedSlider.value = newVal;
        }

        if (photonView.IsMine) {
            health += healthRegen * Time.deltaTime;
            if (health > PlayerBuffController.instance.health) health = PlayerBuffController.instance.health;
            if (myself.type == Entity_Types.PLAYER) instance = this;
        }
    }

    public void ApplyHealthRegen(float rps, float time) {
        StartCoroutine(ApplyRegenForTime(rps, time));
    }

    IEnumerator ApplyRegenForTime(float rps, float time) {
        healthRegen += rps;
        yield return new WaitForSeconds(time);
        healthRegen -= rps;
    }

    public void TakeDamage(float damage) {
        if(health - damage <= 0) {
            myself.isDead = true;
        }
        photonView.RPC("TakeDamagePipe", photonView.Owner, damage);
    }

    [PunRPC]
    public void TakeDamagePipe(float damage) {
        TakeDamageOwner(damage);
    }

    public void TakeDamageOwner(float damage)
    {
        if(damage == 0) return;
        health -= damage;
        if(health > PlayerBuffController.instance.health){
            health = PlayerBuffController.instance.health;
        }else if(health > 0 && damage < 0) {
            GameObject g = GameObjectPooler.instance.Instantiate(floatingText);
            g.transform.position = transform.position;
            g.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = damage.ToString();
            GameObjectPooler.instance.DestroyAfterTime(g, 0.5f);
            UpdateDisplay();
        } else if(health <= 0) {
            myself.isDead = true;
            Die();
        }
    }

    public void UpdateDisplay()
    {
        float val = health / PlayerBuffController.instance.health;
        healthBar.value = val;
        if(funnyDelayedSlider) StartCoroutine("funnyDelay", val);
    }

    private IEnumerator funnyDelay(float newVal) {
        yield return new WaitForSeconds(.5f);
        funnyDelayedSliderTarget = newVal;
    }

    public void Die()
    {
        gameObject.tag = "Untagged";
        myself.isDead = true;
        anim.SetTrigger("Die");
    }

    public void Respawn()
    {
        if (photonView.Owner.UserId == PhotonNetwork.LocalPlayer.UserId)
        {
            RespawnFr();
        }
        else
        {
            photonView.RPC("KillYourself", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void KillYourself()
    {
        Die();
        if (myself.isMine)
        {
            RespawnFr();
        }
    }


    void RespawnFr()
    {
        if (myself.type == Entity_Types.PLAYER)
        {
            if (DeathmatchGMController.instance)
            {
                DeathmatchGMController.instance.RegisterDeath(PhotonNetwork.LocalPlayer.ActorNumber);
            }
            if (TeamSpawnController.instance) TeamSpawnController.instance.SpawnPlayer(PhotonNetwork.LocalPlayer.UserId);
            if (!TeamSpawnController.instance) ChampionDemoController.instance.SpawnBot(pos, rot);
            PlayerManaController.instance.ResetMana();
            CameraFollowLocalPlayer.instance.FindTarget();
        }
        PhotonNetwork.Destroy(gameObject);
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(health);
        }else if(stream.IsReading && !photonView.IsMine) {
            float newHealth = (float) stream.ReceiveNext();
            TakeDamageOwner(health - newHealth);
        }
    }
}