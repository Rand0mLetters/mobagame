using System;
using UnityEngine;


public class Item : ScriptableObject {
    public string itemName;
    [Header("Main")]
    public bool isActive;
    public int price;
    public int sellPrice;
    public bool singleUse;

    [TextArea]
    public string description;
    public Sprite sprite;

    [Header("Buffs")]
    public float damage;
    public float health;
    public float speed;
    public float mana;

    public virtual void OnEquip(PlayerBuffController buffController) {
        buffController.ChangeDamageBuff(damage);
        buffController.ChangeHealthBuff(health);
        buffController.ChangeManaBuff(mana);
        buffController.ChangeMovemementBuff(speed);
    }

    public virtual void OnUnequip(PlayerBuffController buffController) {
        buffController.ChangeDamageBuff(-damage);
        buffController.ChangeHealthBuff(-health);
        buffController.ChangeManaBuff(-mana);
        buffController.ChangeMovemementBuff(-speed);
    }
}
