using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TowerAttackController : MonoBehaviourPunCallbacks
{
    public Transform lightningArc;
    public Entity target;
    public float attackDuration;
    public SphereCollider col;
    Vector3 startPos;
    bool checkCollider;
    bool attacking = false;

    void Start()
    {
        startPos = lightningArc.position;
        InvokeRepeating("Attack", 0, 5);
    }

    private void Update() {
        if (checkCollider) {
            col.enabled = false;
            checkCollider = false;
        } else {
            col.enabled = true;
        }
        if (!target) return;
        if (target.isTeammate) target = null;

        if (attacking && target) {
            lightningArc.position = target.transform.position;
        }
    }


    void Attack()
    {
        if(target && (target.CompareTag("Player") || target.CompareTag("Bot"))) {
            float dist = Vector3.SqrMagnitude(transform.position - target.transform.position);
            if(dist < col.radius * col.radius)
                StartCoroutine("ActuallyAttack");
            else
                checkCollider = true;
        } else {
            checkCollider = true;
        }
    }

    IEnumerator ActuallyAttack() {
        lightningArc.position = target.transform.position;
        attacking = true;
        yield return new WaitForSeconds(attackDuration);
        attacking = false;
        lightningArc.position = startPos;
    }

    private void OnTriggerEnter(Collider other) {
        if (target && (target.CompareTag("Player") || target.CompareTag("Bot"))) return;
        else {
            Entity e = other.GetComponent<Entity>();
            if (e == null) return;
            if (!e.isTeammate) target = e;
        }
    }
}
