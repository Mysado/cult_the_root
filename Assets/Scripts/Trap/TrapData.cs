using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class TrapUpgradesCostDamage
{
    public int UpgradeCost;
    public int DamageForThatLevel;
}

[CreateAssetMenu(menuName = "ScriptableObjects/TrapData")]
public class TrapData : ScriptableObject
{
    public GameObject TrapPrefab;
    public TrapTypes TrapType;
    public bool IsLethal;
    [InfoBox("Cost for level 0 is buy price")]
    public List<TrapUpgradesCostDamage> UpgradeCostAndDamageForThatLevel;
}
