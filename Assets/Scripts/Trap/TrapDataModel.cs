using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapTypes
{
    Boots,
    Swords,
    Maces
}
public class TrapDataModel
{
    public TrapTypes TrapType;
    public bool IsLethal;
    public List<TrapUpgradesCostDamage> UpgradeCostAndDamage;
    public int CurrentLevel;
    
    public TrapDataModel(bool isLethal, List<TrapUpgradesCostDamage> upgradeCostAndDamage, TrapTypes trapType)
    {
        IsLethal = isLethal;
        UpgradeCostAndDamage = upgradeCostAndDamage;
        TrapType = trapType;
        CurrentLevel = 0;
    }
}
