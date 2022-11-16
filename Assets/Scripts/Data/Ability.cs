using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType {
    BASIC,
    PASSIVE,
    UNAIMED,
    AOE,
    TARGETED,
    DIRECTIONAL
}

public enum AttackTarget {
    NONE,
    ENEMY_PLAYERS,
    ENEMY_CREATURES,
    TEAM_PLAYERS,
    TEAM_CREATURES,
    PLAYERS,
    CREATURES
}

public class Ability : ScriptableObject
{
    public string abilityName;
    [TextArea] public string abilityDescription;
    public float manaCost;
    public float cooldown;
    public Sprite icon;
    public AttackType type;
    public AttackTarget target;
    public int maxLevel;

    public AnimationClip clip;
    public KeyCode attackKey;
    private System.Action<Ability> cb;


    public virtual void Activate(System.Action<Ability> cb) { }
    public virtual void Activate(GameObjectPooler pooler, System.Action<Ability> cb) { }
}
