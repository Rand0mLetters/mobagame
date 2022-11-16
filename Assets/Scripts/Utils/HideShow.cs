using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideShow : MonoBehaviour
{
    public GameObject hideObject;
    public GameObject showObject;

    public void Do() {
        if(hideObject) hideObject.SetActive(false);
        if(showObject) showObject.SetActive(true);
    }
}
