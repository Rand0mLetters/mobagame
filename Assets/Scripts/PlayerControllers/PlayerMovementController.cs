using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviourPunCallbacks
{
    public Animator anim;
    public NavMeshAgent agent;
    public GameObject movementEffect;
    public LayerMask mask;
    Entity me;

    void Start()
    {
        me = GetComponent<Entity>();
    }
    
    void Update()
    {
        if (anim)
        {
            float speed = agent.velocity.magnitude / agent.speed;
            anim.SetFloat("Speed", speed);
        }
    }

    public void MoveToMousePosition(InputAction.CallbackContext context)
    {
        if(me.isMine && !me.isDead && context.phase == InputActionPhase.Started && agent.enabled)
        {
            Vector2 mousePosition = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, mask.value))
            {
                if(hit.collider.CompareTag("Terrain"))
                {
                    agent.destination = hit.point;
                    GameObject g = Instantiate(movementEffect, hit.point, Quaternion.identity);
                    Destroy(g, 3);
                }
            }
        }
    }

}
