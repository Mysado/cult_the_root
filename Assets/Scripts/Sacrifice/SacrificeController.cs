using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum SacrificeStates
{
    WalkingToTree,
    IdleAtHole,
    WalkingToExit,
    FallingDown,
    Stunned,
    FightingCultists,
    IdleAtExit
}
public class SacrificeController : MonoBehaviour
{
    public SacrificeStates SacrificeState;
    public SacrificeDataModel SacrificeDataModel;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector3 targetPosition, float walkingDuration, SacrificeStates stateAfterMovement)
    {
        transform.DOMove(targetPosition, walkingDuration).onComplete = () => SacrificeState = stateAfterMovement;
    }

    public void FallIntoHole()
    {
        SacrificeState = SacrificeStates.FallingDown;
    }

    public void DealDamage()
    {
        
    }

}
