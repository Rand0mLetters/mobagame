using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerAttackController : MonoBehaviourPunCallbacks
{
    /*[Header("Cursors")]
    public Texture2D def;
    public Texture2D aim;

    [Header("Other")]
    public GameObject radius;
    public Transform spawnPosition;
    public AnimatorSync anim;
    public AttackData currentAttack;
    public NavMeshAgent agent;
    public LayerMask mask;
    public string characterName;
    bool attacking;
    bool aiming;
    Vector3 lastTarget;
    bool isMine = true;
    GameObject indicator;

    void Awake()
    {
        if (photonView.Owner.UserId != PhotonNetwork.LocalPlayer.UserId)
        {
            isMine = false;
        }
    }


    void Update()
    {
        if (!isMine) return;
        if (attacking)
        {
            if (agent) agent.enabled = false;
            anim.SetTrigger("Attack" + currentAttack.attackKey);
        }

        if(aiming && indicator)
        {
            Vector2 mousePos = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 10000, mask.value, QueryTriggerInteraction.Ignore))
            {
                indicator.transform.position = hit.point;
            }
        }
    }

    public void ClickHandler (InputAction.CallbackContext context)
    {
        if (attacking) return;
        if (!isMine) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (currentAttack == null)
        {
            if(AlertController.instance) AlertController.instance.Alert("No attack selected.");
            return;
        }
        if (isMine && context.phase == InputActionPhase.Started)
        {
            if (currentAttack.aimedAttack)
            {
                if (!aiming)
                {
                    radius.SetActive(true);
                    radius.transform.localScale = Vector3.one * currentAttack.range * 2;
                    indicator = Instantiate(currentAttack.attackIndicator);
                    aiming = true;
                    Cursor.SetCursor(aim, new Vector2(50, 50), CursorMode.Auto);
                }
                else
                {
                    BeginAttack();
                }
            }
            else
            {
                BeginAttack();
            }
            
        }
    }

    public void BeginAttack()
    {
        if (attacking) return;
        if (currentAttack == null) return;
        if (currentAttack.aimedAttack)
        {
            Vector2 mousePosition = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000, mask.value, QueryTriggerInteraction.Ignore))
            {
                if (Vector3.Distance(transform.position, hit.point) > currentAttack.range)
                {
                    AlertController.instance.Alert("oUT oF rAnGE.");
                    return;
                }
                bool hasTag = currentAttack.allowedTargetTags.Length == 0;
                foreach (string tag in currentAttack.allowedTargetTags)
                {
                    if (hit.collider.gameObject.CompareTag(tag)) hasTag = true;
                }
                if (hasTag)
                {
                    bool teammates = TeamController.instance ? TeamController.instance.AreTeammates(
                            PhotonNetwork.LocalPlayer.UserId,
                            hit.collider.GetComponent<PhotonView>().Owner.UserId
                         ) : false;
                    if (
                        !currentAttack.canTargetTeammates && (teammates || !TeamController.instance) && hit.collider.CompareTag("Player")
                    )
                    {
                        AlertController.instance.Alert("CaN'T kIlL yOuR TeAMmAtEs.");
                        return;
                    }
                    lastTarget = hit.point;
                    attacking = true;
                    radius.SetActive(false);
                    if (currentAttack.lookAtTarget) transform.LookAt(hit.point);
                }
                else
                {
                    AlertController.instance.Alert("Invalid target.");
                    return;
                }
            }
        }
        else
        {
            if(!anim) StartCoroutine("SpawnAttack");
            attacking = true;
        }
        Cursor.SetCursor(def, Vector2.zero, CursorMode.Auto);
    }

    public IEnumerator SpawnAimedAttack()
    {
        if (currentAttack == null) yield break;
        if(!isMine) yield break;
        if(indicator) Destroy(indicator);
        attacking = true;
        yield return new WaitForSeconds(currentAttack.delayTime);
        GameObject go = PhotonNetwork.Instantiate(characterName + "/" + "Attacks/" + currentAttack.attackObject.name, spawnPosition.position, spawnPosition.rotation);
        go.SendMessage("SetTarget", lastTarget);
        if (currentAttack.parentToSpawn) go.transform.SetParent(spawnPosition);
        yield return new WaitForSeconds(currentAttack.attackLength);
        PhotonNetwork.Destroy(go);
        attacking = false;
    }

    public IEnumerator SpawnAttack()
    {
        if (!isMine) yield break;
        if(currentAttack == null) yield break;
        attacking = true;
        yield return new WaitForSeconds(currentAttack.delayTime);
        GameObject go = PhotonNetwork.Instantiate(characterName + "/" + "Attacks/" + currentAttack.attackObject.name, spawnPosition.position, spawnPosition.rotation);
        if (currentAttack.parentToSpawn) go.transform.SetParent(spawnPosition);
        yield return new WaitForSeconds(currentAttack.attackLength);
        PhotonNetwork.Destroy(go);
        attacking = false;
    }

    public void CancelAttack()
    {
        aiming = false;
        attacking = false;
        if(radius) radius.SetActive(false);
        lastTarget = Vector3.zero;
        Cursor.SetCursor(def, Vector2.zero, CursorMode.Auto);
        EquipAttack(null);
    }

    public void EquipAttack(AttackData newAttack)
    {
        if (!isMine) return;
        if(indicator) Destroy(indicator);
        if(radius) radius.SetActive(false);
        lastTarget = Vector3.zero;
        if(aiming) aiming = false;
        currentAttack = newAttack;
        Cursor.SetCursor(def, Vector2.zero, CursorMode.Auto);
    }

    public void EnableAgent()
    {
        agent.enabled = true;
        attacking = false;
        aiming = false;
        Cursor.SetCursor(def, Vector2.zero, CursorMode.Auto);
    }*/
}
