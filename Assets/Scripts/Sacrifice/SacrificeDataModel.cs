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
    public int PercentageHpLossToStun;
    public int CultistsKillAmount;
    public int WalkingSpeed;
    public int ExpWorth;
    public int WaitingTimeAtTree;

    public SacrificeDataModel(int hp, int percentage, int cultistsKillAmount, int walkingSpeed, int expWorth, SacrificeTypes sacrificeType, int waitingTimeAtTree)
    {
        Hp = hp;
        PercentageHpLossToStun = percentage;
        CultistsKillAmount = cultistsKillAmount;
        WalkingSpeed = walkingSpeed;
        ExpWorth = expWorth;
        SacrificeType = sacrificeType;
        WaitingTimeAtTree = waitingTimeAtTree;
    }
}
