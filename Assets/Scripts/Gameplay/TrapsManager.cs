using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrapsManager : MonoBehaviour
{
    [SerializeField] private BottomHoleController bottomHoleController;
    private List<TrapController> traps = new();
    public event Action OnSacrificeReachedBottom;
    public event Action<TrapTypes> OnTrapHit;

    private void Start()
    {
        bottomHoleController.OnSacrificeReachedBottom += OnSacrificeReachedBottom.Invoke;
    }

    public void BuyTrap(Transform flipperSpotTransform, TrapData trapData)
    {
        var trap = Instantiate(trapData.TrapPrefab, flipperSpotTransform.position,quaternion.identity, transform).GetComponent<TrapController>();
        trap.OnTrapHit += OnTrapHitEvent;
        trap.TrapDataModel = CreateTrapDataModel(trapData);
        traps.Add(trap);
    }

    public void UpgradeTrap(TrapController trapToUpgrade)
    {
        trapToUpgrade.UpgradeTrap();
    }

    public void ChangeSpinDirection(TrapController trapToChange)
    {
        trapToChange.ChangeSpinDirection();
    }

    private TrapDataModel CreateTrapDataModel(TrapData trapData)
    {
        return new TrapDataModel(
            trapData.IsLethal,
            trapData.UpgradeCostAndDamageForThatLevel,
            trapData.TrapType
        ); 
    }

    private void OnTrapHitEvent(TrapTypes trapType)
    {
        OnTrapHit?.Invoke(trapType);
    }
}
