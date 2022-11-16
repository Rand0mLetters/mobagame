using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackSwitchController : MonoBehaviourPunCallbacks
{
    public Ability[] attacks;
    public AttackController controller;
    public MatchAttackData matchData;

    private IEnumerator Start() {
        yield return new WaitForEndOfFrame();
        matchData = new MatchAttackData();
        MAD[] mad = new MAD[attacks.Length];
        for(int i = 0; i < mad.Length; i++) {
            MAD m = new();
            m.data = attacks[i];
            m.maxLevel = attacks[i].maxLevel;
            m.level = 0;
            mad[i] = m;
        }
        matchData.attackStates = mad;
        PlayerStatesSynchronizer.instance.matchData.attackData = matchData;
    }

    public void SwitchKeyPress(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;
        float val = context.ReadValue<float>();
        SwitchAttack((int) val);
    }

    public void SwitchAttack(int attackIndex)
    {
        // Debug.Log("Switched to attack " + matchData.attackStates[attackIndex].data.attackName);
        // controller.EquipAttack(attacks[attackIndex]);
    }
}
