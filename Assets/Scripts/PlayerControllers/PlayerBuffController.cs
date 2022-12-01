using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffController : MonoBehaviour
{
    public static PlayerBuffController instance;
    public float damage;
    public float movement;
    public float health;
    public float mana;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        XPController.instance.onLevelUp = OnLevelUp;
    }

    public void OnLevelUp(int newLevel) {
        ChangeHealthBuff(100);
        ChangeDamageBuff(10);
        ChangeMovemementBuff(1);
        ChangeManaBuff(25);
    }

    public void ChangeDamageBuff(float change) { damage += change; }

    public void ChangeMovemementBuff(float change) { movement += change; }

    public void ChangeHealthBuff(float change) {
        float curPercentage = PlayerHealthController.instance.health / health;
        health += change;
        PlayerHealthController.instance.health = curPercentage * health;
    }

    public void ChangeManaBuff(float change) {
        float curPercentage = PlayerManaController.instance.mana / mana;
        mana += change;
        PlayerManaController.instance.mana = curPercentage * mana;
    }
}
