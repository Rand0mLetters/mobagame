using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Item/Consumable", fileName = "new Consumable")]
public class Consumable : Item
{
    public float healthRegen;
    public float healthRegenTime;

    public float manaRegen;
    public float manaRegenTime;

    public void Use(PlayerHealthController health, PlayerManaController mana) {
        health.ApplyHealthRegen(healthRegen, healthRegenTime);
        mana.AddManaRegen(manaRegen, manaRegenTime);
    }
}
