using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SacrificeTypes
{
    Peasant,
    Paladin,
    Bear
}
public class SacrificeDataModel
{
    public SacrificeTypes SacrificeType;
    public int Hp;
    public int MaxHp;
    public float PercentageHpLossToStun;
    public int CultistsKillAmount;
    public int WalkingSpeed;
    public int ExpWorth;
    public int WaitingTimeAtTree;
    
    public SacrificeDataModel(int hp, float percentage, int cultistsKillAmount, int walkingSpeed, int expWorth, SacrificeTypes sacrificeType, int waitingTimeAtTree)
    {
        Hp = hp;
        MaxHp = hp;
        PercentageHpLossToStun = percentage;
        CultistsKillAmount = cultistsKillAmount;
        WalkingSpeed = walkingSpeed;
        ExpWorth = expWorth;
        SacrificeType = sacrificeType;
        WaitingTimeAtTree = waitingTimeAtTree;
    }
}
