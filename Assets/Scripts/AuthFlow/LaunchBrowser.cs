using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchBrowser : MonoBehaviour
{
    public string url;

    public void OpenBrowser()
    {
        Application.OpenURL(url);
    }
}
