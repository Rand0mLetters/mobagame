using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Photon.Pun;

[System.Serializable]
public class AbilityUI {
    public GameObject go;
    public Image icon;
    public TextMeshProUGUI cooldownText;
    public Image radialSpinThing;
}

[System.Serializable]
public class InventorySlotUI {
    public Image icon;

    public void SetImage(Sprite sprite) {
        icon.sprite = sprite;
    }
}

public class PlayerStatsDisplay : MonoBehaviour
{
    [Header("Random")]
    public TextMeshProUGUI pingDisplay;

    [Header("Health")]
    public Slider healthBar;
    public TextMeshProUGUI healthDisplay;
    public Image[] deadnesses;
    public Color[] colors;

    [Header("Mana")]
    public Slider manaBar;
    public TextMeshProUGUI manaDisplay;

    [Header("Abilities")]
    public AbilityUI[] abilities;

    [Header("Inventory")]
    public Sprite emptySlot;
    public InventorySlotUI[] inventorySlots;

    private void Start() {
        UpdateInventoryUI(new List<Item>());
        for (int i = 0; i < abilities.Length; i++) {
            if (i >= AttackController.instance.abilities.Length) {
                abilities[i].go.SetActive(false);
                continue;
            } else {
                if (AttackController.instance.abilities[i].icon) {
                    abilities[i].icon.sprite = AttackController.instance.abilities[i].icon;
                }
            }
        }
    }

    private void Update() {
        pingDisplay.text = "Ping: " + PhotonNetwork.GetPing();
        if (PlayerHealthController.instance) {
            healthDisplay.text = (int)(PlayerHealthController.instance.health + 0.5f) + " / " + PlayerBuffController.instance.health;
            float percentage = PlayerHealthController.instance.health / PlayerBuffController.instance.health;
            if (percentage < 0) percentage = 0;
            healthBar.value = percentage;
            Color color;
            if (percentage > 0.75f){
                color = colors[0];
                healthDisplay.color = Color.white;
            }
            else if (percentage > 0.35f) {
                color = colors[1];
                healthDisplay.color = color;
            } else {
                color = colors[2];
                healthDisplay.color = color;
            }
            for(int i = 0; i < deadnesses.Length; i++) {
                deadnesses[i].color = color;
            }
        }
        if (PlayerManaController.instance) {
            manaBar.value = PlayerManaController.instance.mana / PlayerBuffController.instance.mana;
            manaDisplay.text = (int) (PlayerManaController.instance.mana + 0.5f) + " / " + PlayerBuffController.instance.mana;
        }
        if (AttackController.instance) {
            for(int i = 0; i < abilities.Length; i++) {
                if(i >= AttackController.instance.abilities.Length) {
                    abilities[i].go.SetActive(false);
                    continue;
                }
                if (AttackController.instance.cooldowns[i] >= 0) {
                    abilities[i].cooldownText.text = ((int) (AttackController.instance.cooldowns[i] + 0.5f)).ToString();
                    abilities[i].radialSpinThing.fillAmount = AttackController.instance.cooldowns[i] / AttackController.instance.abilities[i].cooldown;
                } else {
                    abilities[i].cooldownText.text = "";
                    abilities[i].radialSpinThing.fillAmount = 0;
                }
            }
        }
    }

    public void UpdateInventoryUI(List<Item> items) {
        for(int i = 0; i < inventorySlots.Length; i++) {
            inventorySlots[i].SetImage(i < items.Count ? items[i].sprite : emptySlot);
        }
    }
}
