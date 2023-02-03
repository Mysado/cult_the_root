using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Difficulty
{
    Easy,
    MediumEasy,
    Medium,
    MediumHard,
    Hard,
    HardHard,
}
public class SacrificeManager : MonoBehaviour
{
    [SerializeField] private Transform TreePosition;
    [SerializeField] private Transform ExitPosition;
    [SerializeField] private List<SacrificeData> Sacrifices;
    [SerializeField] private GameObject SacrificeObject;
    private SacrificeController currentSacrifice;
    private Difficulty currentDifficulty;
    private bool startedExitTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentSacrifice)
        {
            SpawnSacrifice();
            return;
        }
        
        if (currentSacrifice && !startedExitTimer && currentSacrifice.SacrificeState == SacrificeStates.IdleAtHole)
        {
            startedExitTimer = true;
            var myFloat = 1;
            DOTween.To(()=> myFloat, x=> myFloat = x, 52, currentSacrifice.SacrificeDataModel.WaitingTimeAtTree).onComplete = MoveSacrificeToExit;
        }

        if (currentSacrifice && currentSacrifice.SacrificeState == SacrificeStates.IdleAtExit)
        {
            DespawnSacrifice();
        }
    }

    private void SpawnSacrifice()
    {
        var sacrifices = Sacrifices.Where(x => (int)x.MinimumDifficultyNeededToAppear <= (int)currentDifficulty).ToArray();
        var randomSacrifice = sacrifices[Random.Range(0, sacrifices.Length)];
        var dataModel = GenerateSacrifice(randomSacrifice);
        currentSacrifice = Instantiate(SacrificeObject, ExitPosition.position, quaternion.identity).GetComponent<SacrificeController>();
        currentSacrifice.SacrificeDataModel = dataModel;
        currentSacrifice.Move(TreePosition.position, currentSacrifice.SacrificeDataModel.WalkingSpeed, SacrificeStates.IdleAtHole);
        currentSacrifice.SacrificeState = SacrificeStates.WalkingToTree;
    }

    private void MoveSacrificeToExit()
    {
        currentSacrifice.Move(ExitPosition.position, currentSacrifice.SacrificeDataModel.WalkingSpeed, SacrificeStates.IdleAtExit);
        currentSacrifice.SacrificeState = SacrificeStates.WalkingToExit;
        startedExitTimer = false;
    }

    private void DespawnSacrifice()
    {
        Destroy(currentSacrifice.gameObject);
    }

    private SacrificeDataModel GenerateSacrifice(SacrificeData sacrificeData)
    {
        var difficultyMultiplier = (int)currentDifficulty;
        return new SacrificeDataModel(
             Mathf.CeilToInt(sacrificeData.Hp * sacrificeData.HpMultiplierOnDifficulty * difficultyMultiplier),
            sacrificeData.PercentageHpLossToStun,
             Mathf.CeilToInt(sacrificeData.CultistsKillAmount * sacrificeData.CultistsKillAmountOnDifficulty * difficultyMultiplier),
            sacrificeData.WalkingSpeed,
             Mathf.CeilToInt(sacrificeData.ExpWorth * sacrificeData.ExpMultiplierOnDifficulty * difficultyMultiplier),
            sacrificeData.SacrificeType,
             sacrificeData.WaitingTimeAtTree
        );
    }
}
