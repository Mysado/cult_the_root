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
    private TweenCallback returnTween;
    private bool inProgressOfSpawning;

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

    public void SacrificeSacrificed()
    {
        currentSacrifice = null;
        ResetValues();
    }

    private void Update()
    {
        if (!currentSacrifice && isInitialized && !inProgressOfSpawning)
        {
            inProgressOfSpawning = true;
            SpawnSacrifice();
            return;
        }
        
        if (currentSacrifice && !startedExitTimer && currentSacrifice.SacrificeState == SacrificeStates.IdleAtHole)
        {
            startedExitTimer = true;
            var myFloat = 1;
            returnTween = DOTween.To(()=> myFloat, x=> myFloat = x, 52, currentSacrifice.SacrificeDataModel.WaitingTimeAtTree).onComplete = MoveSacrificeToExit;
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
        inProgressOfSpawning = false;
    }

    private void MoveSacrificeToExit()
    {
        if(currentSacrifice.SacrificeState != SacrificeStates.IdleAtHole)
            return;
        currentSacrifice.Move(ExitPosition.position, currentSacrifice.SacrificeDataModel.WalkingSpeed, SacrificeStates.IdleAtExit);
        currentSacrifice.SacrificeState = SacrificeStates.WalkingToExit;
        startedExitTimer = false;
        currentSacrifice.transform.DOLocalRotate(new Vector3(0.0f, 180.0f, 0.0f), 1.0f);
    }

    private void DespawnSacrifice()
    {
        Destroy(currentSacrifice.gameObject);
        ResetValues();
    }

    private SacrificeDataModel GenerateSacrifice(SacrificeData sacrificeData)
    {
        var difficultyMultiplier = gameManager.GetCurrentDifficulty();
        Debug.Log(Mathf.CeilToInt(sacrificeData.Hp * sacrificeData.HpMultiplierOnDifficulty * difficultyMultiplier));
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

    private void ResetValues()
    {
        startedExitTimer = false;
        DOTween.Kill(returnTween);
        inProgressOfSpawning = false;
    }
}
