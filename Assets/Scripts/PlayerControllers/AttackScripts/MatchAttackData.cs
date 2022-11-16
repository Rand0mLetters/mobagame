using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MAD {
    public Ability data;
    public int level;
    public int maxLevel;
}

[System.Serializable]
public class MatchAttackData
{
    public MAD[] attackStates;
}
