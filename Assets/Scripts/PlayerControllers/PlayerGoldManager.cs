using UnityEngine;

public class PlayerGoldManager : MonoBehaviour {
    public static PlayerGoldManager instance;
    public TMPro.TextMeshProUGUI goldDisplay;
    PlayerMatchData data;

    void Awake() {
        instance = this;
    }

    void Start() {
        data = PlayerStatesSynchronizer.instance.matchData;
        UpdateDisplay();
    }

    public bool SubtractMoney(int amount) {
        if (HasEnoughMoney(amount)) {
            data.gold -= amount;
            UpdateDisplay();
            return true;
        }
        return false;
    }

    public void AddMoney(int amount) {
        data.gold += amount;
        UpdateDisplay();
    }

    public bool HasEnoughMoney(int amount) {
        return data.gold > amount;
    }

    void UpdateDisplay() {
        if (!goldDisplay) {
            Debug.Log("no display");
            return;
        }
        if (data == null) {
            Debug.Log("no data");
            return;
        }
        goldDisplay.text = data.gold.ToString();
    }
}