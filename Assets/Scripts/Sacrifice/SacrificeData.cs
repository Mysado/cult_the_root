using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/SacrificeData")]
public class SacrificeData : ScriptableObject
{
    public GameObject SacrificePrefab;
    public SacrificeTypes SacrificeType;
    public int MinimumDifficultyNeededToAppear;
    public int Hp;
    public float PercentageHpLossToStun;
    [InfoBox("cultists amount per 10% above stun threshold")]
    public int CultistsKillAmount;
    [InfoBox("The lower the better")]
    public int WalkingSpeed;
    public int ExpWorth;
    public int WaitingTimeAtTree;

    public float ExpMultiplierOnDifficulty;
    public float CultistsKillAmountOnDifficulty;
    public float HpMultiplierOnDifficulty;

    public List<AudioClip> ScreamsOfPain;
}
