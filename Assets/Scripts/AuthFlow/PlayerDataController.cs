using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataController : MonoBehaviour
{
    public LoginResult data;
    public string username;
    public static PlayerDataController instance;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        username = PlayerPrefs.GetString("username");
        if(username == null || username.Length == 0) {
            username = GenerateRandomString(10);
            PlayerPrefs.SetString("username", username);
        }
    }

    private string GenerateRandomString(int length) {
        string s = "";
        for(int i = 0; i < length; i++) {
            s += UnityEngine.Random.Range(0, 10);
        }
        return s;
    }

    private void Update()
    {
        if (!Application.runInBackground)
        {
            Application.runInBackground = true;
        }
    }

    public void SetData(LoginResult res)
    {
        data = res;
    }

    public void SetUsername(string s) {
        username = s;
        PlayerPrefs.SetString("username", username);
    }
}
