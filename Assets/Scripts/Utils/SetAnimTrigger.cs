using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimTrigger : MonoBehaviour
{
    public Animator anim;

    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }
}
