using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TrapData")]

public class TrapUpgradesCostDamage
{
    public int UpgradeCost;
    public int DamageForThatLevel;
}
public class TrapData : ScriptableObject
{
    public TrapTypes TrapType;
    public bool IsLethal;
    [InfoBox("Cost for 0 is buy price")]
    public List<TrapUpgradesCostDamage> UpgradeCostAndDamageForThatLevel;
}
