using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class CameraFollowLocalPlayer : MonoBehaviour
{
    public Transform target;
    public float delay = 0;
    public float bounds = 50;
    bool follow = true;
    Vector3 curTarget;
    public Vector3 camTopBounds = new Vector3(45, 0, 45);
    public Vector3 camBottomBounds = new Vector3(-45, 0, -35);
    public static CameraFollowLocalPlayer instance;

    private void Awake()
    {
         instance = this;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(delay);
        FindTarget();
    }

    public void FindTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in players)
        {
            if (go.GetComponent<PhotonView>().Owner.UserId == PhotonNetwork.LocalPlayer.UserId)
            {
                target = go.transform;
                break;
            }
        }
    }

    void Update()
    {
        if (follow && target)
        {
            curTarget = target.position;
        }
        else
        {
            Vector2 mousePos = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
            if (mousePos.y < bounds)
            {
                curTarget += Vector3.back;
            }
            if (mousePos.y > Screen.height - bounds)
            {
                curTarget += Vector3.forward;
            }
            if (mousePos.x < bounds)
            {
                curTarget += Vector3.left;
            }
            if (mousePos.x > Screen.width - bounds)
            {
                curTarget += Vector3.right;
            }
        }
        transform.position = Vector3.Lerp(transform.position, curTarget, Time.deltaTime * 2);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, camBottomBounds.x, camTopBounds.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, camBottomBounds.z, camTopBounds.z)
        );
    }

    public void Center()
    {
        curTarget = target.position;
    }

    public void FollowOrUnFollow(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            follow = !follow;
        }
    }
}
