using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager instance;
    PlayerMatchData data;
    public AudioClip errorSound;
    public PlayerStatsDisplay ui;

    private void Awake() {
        instance = this;
    }

    void Start() {
        data = PlayerStatesSynchronizer.instance.matchData;
        ui = GetComponent<PlayerStatsDisplay>();
    }

    public void BuyItem(Item item) {
        if (data.inventory.Count >= 6) {
            if (AudioManager.instance) AudioManager.instance.PlaySound(errorSound);
            if (AlertController.instance) AlertController.instance.Alert("Inventory full");
            return;
        }
        if (!PlayerGoldManager.instance.SubtractMoney(item.price)) {
            if (AudioManager.instance) AudioManager.instance.PlaySound(errorSound);
            if (AlertController.instance) AlertController.instance.Alert("Not enough gold");
        } else {
            data.inventory.Add(item);
            item.OnEquip(PlayerBuffController.instance);
            ui.UpdateInventoryUI(data.inventory);
        }
    }

    public void UseItem(int index) {
        if (index >= data.inventory.Count) {
            if (AudioManager.instance) AudioManager.instance.PlaySound(errorSound);
            if (AlertController.instance) AlertController.instance.Alert("Can't use that");
            return;
        }
        HAHAHAUse(data.inventory[index]);
        if (data.inventory[index].singleUse) {
            RemoveItem(index);
        }
    }

    private void HAHAHAUse(Item item) {
        if (item is Consumable consumable) {
            consumable.Use(PlayerHealthController.instance, PlayerManaController.instance);
        }
    }

    public void SellItem(int index) {
        if (index >= data.inventory.Count) {
            if (AudioManager.instance) AudioManager.instance.PlaySound(errorSound);
            if (AlertController.instance) AlertController.instance.Alert("Can't sell that");
            return;
        }
        PlayerGoldManager.instance.AddMoney(data.inventory[index].sellPrice);
        RemoveItem(index);
    }

    public void RemoveItem(int index) {
        data.inventory[index].OnUnequip(PlayerBuffController.instance);
        data.inventory.RemoveAt(index);
        ui.UpdateInventoryUI(data.inventory);
    }
}