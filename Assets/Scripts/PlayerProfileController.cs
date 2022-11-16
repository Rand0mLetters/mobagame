using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfileController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI info;
    public TMPro.TMP_InputField inputField;

    void Start()
    {
        if (PlayerDataController.instance == null || PlayerDataController.instance.data == null)
        {
            Application.Quit();
        }
        else
        {
            info.text = PlayerDataController.instance.username;
            inputField.text = PlayerDataController.instance.username;
        }
    }

    public void OnChange(string newVariable) {
        if (newVariable.Trim().Length == 0) return;
        PlayerDataController.instance.SetUsername(newVariable.Trim());
    }
}
