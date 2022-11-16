using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[System.Serializable]
public class CursorData {
    public Texture2D cursor;
    public Vector2 offset;
}

public class AttackController : MonoBehaviourPunCallbacks
{
    public static AttackController instance;
    [Header("Cursors")]
    public CursorData defaultCursor;
    public CursorData aimCursor;

    [Header("Base Attack")]
    public MAD baseAttack;
    public bool isMelee;
    public GameObject projectile;
    public bool isBaseAttacking = false;
    public float range;
    public float pSpeed;
    bool canAttack = true;
    float lastAttackTime;
    public Entity target;
    List<GameObject> projectiles;

    [Header("Attacks")]
    public Ability[] abilities;
    public float[] cooldowns;

    [Header("Other")]
    public LayerMask mask;
    public AudioClip errorSound;

    [Header("Components")]
    public NavMeshAgent agent;
    public Animator anim;
    public string characterName;

    Entity myself;

    void Awake()
    {
        myself = GetComponent<Entity>();
        projectiles = new List<GameObject>();
        cooldowns = new float[abilities.Length];
    }


    void Update()
    {
        if (!myself.isMine) return;
        if(myself.isDead) return;
        if (lastAttackTime + baseAttack.data.clip.length > Time.time) {
            agent.destination = transform.position;
        }
        if (target && !target.isDead) {
            transform.LookAt(target.transform);
            if (target.isTeammate) {
                agent.destination = target.transform.position - target.transform.forward * 2;
            } else if(Vector3.SqrMagnitude(transform.position - target.transform.position) < range * range){
                // attack or shit
                if(agent && agent.enabled) agent.destination = transform.position;
                anim.SetTrigger("Base Attack");
                if (isMelee && canAttack) {
                    target.damageable.TryTakeDamage(PlayerBuffController.instance.damage, photonView.Owner.UserId);
                    canAttack = false;
                    lastAttackTime = Time.time;
                }
            } else {
                // move to last known position
                if(agent.enabled) agent.destination = target.transform.position;
            }
            for(int i = 0; i < projectiles.Count; i++) {
                GameObject obj = projectiles[i];
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, target.transform.position + Vector3.up, pSpeed * Time.deltaTime);
                obj.transform.transform.LookAt(target.transform);
                if(Vector3.SqrMagnitude(obj.transform.position - target.transform.position) < 1) {
                    target.damageable.TryTakeDamage(PlayerBuffController.instance.damage, photonView.Owner.UserId);
                    if (target.isDead) {
                        GiveMoney(target);
                    }
                    projectiles.Remove(obj);
                    PhotonNetwork.Destroy(obj);
                    i--;
                }
            }
        } else {
            if(projectiles.Count > 0) {
                foreach(GameObject p in projectiles) {
                    PhotonNetwork.Destroy(p);
                }
                projectiles.Clear();
            }
        }
        if(lastAttackTime + baseAttack.data.clip.length < Time.time) {
            canAttack = true;
        }

        if (!myself.isDead) {
            for(int i = 0; i < abilities.Length; i++) {
                bool keyDown = Input.GetKeyDown(abilities[i].attackKey);
                if (keyDown) {
                    if (cooldowns[i] <= 0 && PlayerManaController.instance.HasEnoughMana(abilities[i].manaCost)) {
                        abilities[i].Activate(GameObjectPooler.instance, FinalizeAbility);
                    } else if(cooldowns[i] > 0){
                        if (AudioManager.instance) AudioManager.instance.PlaySound(errorSound);
                        if (AlertController.instance) AlertController.instance.Alert("Ability not ready!");
                        Debug.Log("Can't use \"" + abilities[i].abilityName + "\"! Ability is still on cooldown, with " + cooldowns[i] + " seconds remaining!");
                    } else {
                        if (AudioManager.instance) AudioManager.instance.PlaySound(errorSound);
                        if (AlertController.instance) AlertController.instance.Alert("Not enough mana!");
                        Debug.Log("Can't use \"" + abilities[i].abilityName + "\"! Not enough mana to use ability: need " + abilities[i].manaCost + ", has: " + PlayerManaController.instance.mana);
                    }
                }
                
            }
            for(int i = 0; i < cooldowns.Length; i++) {
                cooldowns[i] -= Time.deltaTime;
            }
        }
        if (myself.isMine) instance = this;
    }

    void FinalizeAbility(Ability a) {
        int index = -1;
        for(int i = 0; i < abilities.Length; i++) {
            if (abilities[i].abilityName == a.abilityName) {
                index = i;
                break;
            }
        }
        if (PlayerManaController.instance.UseMana(a.manaCost)) {
            cooldowns[index] = abilities[index].cooldown;
        }
    }

    void GiveMoney(Entity e) {
        e.hasGivenReward = true;
        PlayerGoldManager.instance.AddMoney((int) e.type);
        Debug.Log("Gived player " + (int) e.type + " gold for killing entity of type " + e.type);
    }

    #region INPUTS

    public void OnRightClick(InputAction.CallbackContext context) {
        if(myself.isMine && context.phase == InputActionPhase.Started) {
            RaycastHit hit;
            Vector2 mousePosition = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            if(Physics.Raycast(ray, out hit, 1000, mask.value, QueryTriggerInteraction.Ignore)) {
                Entity e = hit.collider.gameObject.GetComponent<Entity>();
                if (e != null) {
                    target = e;
                } else {
                    // un follow, stop attacking
                    target = null;
                }
            }
        }
    }
    #endregion

    #region SPAWN ATTACKS
    public void SpawnAttack() {
        if (target == null) return;
        if (!myself.isMine) return;
        GameObject p = PhotonNetwork.Instantiate(characterName + "/Attacks/" + projectile.name, transform.position, transform.rotation);
        projectiles.Add(p);
    }
    #endregion
}
