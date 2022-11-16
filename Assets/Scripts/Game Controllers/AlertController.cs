using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertController : MonoBehaviour
{
    public static AlertController instance;
    public TextMeshProUGUI text;
    public Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void Alert(string message)
    {
        text.text = message;
        anim.SetTrigger("Alert");
    }
}
