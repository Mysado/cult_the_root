using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Gameplay;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SacrificeManager : MonoBehaviour
{
    [SerializeField] private Transform TreePosition;
    [SerializeField] private Transform ExitPosition;
    
    public event Action<Transform> OnSacrificeSpawned; 

    private SacrificeController currentSacrifice;
    private GameManager gameManager;
    private bool startedExitTimer;
    private bool isInitialized;
    // Start is called before the first frame update
    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        isInitialized = true;
    }
    
    public Transform GetSacrificeTransform()
    {
        return currentSacrifice.transform;
    }

    public SacrificeController GetSacrificeController()
    {
        return currentSacrifice;
    }

    private void Update()
    {
        if (!currentSacrifice || !isInitialized)
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
        var sacrifices = gameManager.GameDataHolder.SacrificeDatas.Where(x => x.MinimumDifficultyNeededToAppear <= gameManager.GetCurrentDifficulty()).ToArray();
        var randomSacrifice = sacrifices[Random.Range(0, sacrifices.Length)];
        var dataModel = GenerateSacrifice(randomSacrifice);
        currentSacrifice = Instantiate(randomSacrifice.SacrificePrefab, ExitPosition.position, quaternion.identity).GetComponent<SacrificeController>();
        currentSacrifice.SacrificeDataModel = dataModel;
        currentSacrifice.Move(TreePosition.position, currentSacrifice.SacrificeDataModel.WalkingSpeed, SacrificeStates.IdleAtHole);
        currentSacrifice.SacrificeState = SacrificeStates.WalkingToTree;
        OnSacrificeSpawned?.Invoke(currentSacrifice.transform);
    }

    private void MoveSacrificeToExit()
    {
        if(currentSacrifice.SacrificeState != SacrificeStates.IdleAtHole)
            return;
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
        var difficultyMultiplier = gameManager.GetCurrentDifficulty();
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
