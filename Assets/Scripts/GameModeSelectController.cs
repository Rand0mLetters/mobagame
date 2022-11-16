using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameModeSelectController : MonoBehaviour
{
    public TextMeshProUGUI gameModeName;
    public TextMeshProUGUI gameModeDescription;
    public MenuConnectionController controller;

    void Start()
    {
        UpdateDisplay(controller.gameModes[0]);
    }

    public void SelectGameMode(int mode)
    {
        controller.sI = mode;
        UpdateDisplay(controller.gameModes[mode]);
    }

    public void UpdateDisplay(GameModeData data)
    {
        gameModeName.text = data.gameModeName;
        gameModeDescription.text = data.gameModeDescription;
    }
}
