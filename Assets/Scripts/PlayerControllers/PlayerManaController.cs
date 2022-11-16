using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManaController : MonoBehaviour
{
    public float mana;
    public float manaRegen;
    public static PlayerManaController instance;
    float baseRegen;

    public void Awake() {
        instance = this;
        baseRegen = manaRegen;
    }

    private void Start() {
        mana = PlayerBuffController.instance.mana;
    }

    private void Update() {
        mana += manaRegen * Time.deltaTime;
        if(mana > PlayerBuffController.instance.mana) mana = PlayerBuffController.instance.mana;
    }

    public void ResetMana() {
        mana = PlayerBuffController.instance.mana;
        manaRegen = baseRegen;
    }

    public bool UseMana(float amount) {
        if (HasEnoughMana(amount)) {
            mana -= amount;
            return true;
        }
        return false;
    }

    public bool HasEnoughMana(float amount) {
        return mana >= amount;
    }

    public void AddManaRegen(float amount, float time) {
        StartCoroutine(ApplyRegenForTime(amount, time));
    }

    IEnumerator ApplyRegenForTime(float amount, float time) {
        manaRegen += amount;
        yield return new WaitForSeconds(time);
        manaRegen -= amount;
    }
}
