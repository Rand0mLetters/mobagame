using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum LANE {
    BOTTOM = 0,
    MID = 1,
    TOP = 2,
}

[RequireComponent(typeof(Entity))]
public class CreepBehaviourController : MonoBehaviourPun
{
    public AnimatorSync anim;
    public NavMeshAgent agent;
    public Collider col;
    public AnimationClip clip;
    public Transform[] waypoints;

    [Header("Values")]
    public float attackRange;
    public float unalertRadius;
    public float damage;
    public Vector3 destination;


    public Entity currentTarget;
    Entity myself;
    float attackTime;
    float lastAttackTime;
    public int curWaypointIndex = 0;
    bool arcadianLastFrame;
    Vector3 posLastFrame;


    private void Awake() {
        myself = GetComponent<Entity>();
        attackTime = clip.length;
        posLastFrame = transform.position;
    }

    void Update()
    {
        if(arcadianLastFrame != myself.isArcadian) {
            arcadianLastFrame = myself.isArcadian;
            waypoints = Reverse(waypoints);
        }
        if (myself.isDead) {
            if(agent && agent.enabled) agent.destination = transform.position;
            return;
        }
        if (currentTarget && !currentTarget.CompareTag("Untagged") && currentTarget.isArcadian != myself.isArcadian)
        {
            float dist = Vector3.SqrMagnitude(transform.position - currentTarget.transform.position);
            col.enabled = false;
            if (dist < attackRange * attackRange)
            {
                agent.destination = posLastFrame;
                posLastFrame = transform.position;
                transform.LookAt(currentTarget.transform, Vector3.up);
                anim.SetTrigger("Attack");

                // attack
                if(Time.time > lastAttackTime + attackTime) {
                    lastAttackTime = Time.time;
                    currentTarget.SendMessage("TakeDamage", damage);
                }
            }else if(dist < unalertRadius * unalertRadius){
                if(agent.enabled) agent.destination = currentTarget.transform.position;
                anim.SetTrigger("Move");
            }else{
                currentTarget = null;
                anim.SetTrigger("Move");
            }
        }else{
            col.enabled = true;
            currentTarget = null;
            if(agent.enabled && waypoints.Length > 0) {
                agent.destination = waypoints[curWaypointIndex].position;
                if(Vector3.SqrMagnitude(transform.position - waypoints[curWaypointIndex].position) < 4) {
                    curWaypointIndex += 1;
                    curWaypointIndex %= waypoints.Length;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!myself.isMine) return;
        if (currentTarget) return;
        Entity e = other.gameObject.GetComponent<Entity>();
        if (e == null) return;
        if (e.isArcadian == myself.isArcadian) return;
        currentTarget = e;
    }

    public void SetLane(LANE lane) {
        // send RPC
        photonView.RPC("SetLaneReal", RpcTarget.All, lane);
    }

    [PunRPC]
    public void SetLaneReal(LANE lane) {
        // is arcadia?
        bool isArcadia = myself.isArcadian;
        // get waypoints
        waypoints = CreepSpawnController.instance.lanes[(int) lane].waypoints;
        // reverse if xanadu
        if (!isArcadia) waypoints = Reverse(waypoints);
    }

    Transform[] Reverse(Transform[] transforms) {
        Transform[] result = new Transform[transforms.Length];
        for(int i = 0; i < transforms.Length/ 2; i++) {
            Transform temp = transforms[transforms.Length - 1 - i];
            result[result.Length - 1 - i] = transforms[i];
            result[i] = temp;
        }
        return result;
    }
}
