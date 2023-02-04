using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SacrificeStates
{
    WalkingToTree,
    IdleAtHole,
    WalkingToExit,
    FallingDown,
    Stunned,
    FightingCultists,
    IdleAtExit,
    Dead
}
public class SacrificeController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private SacrificeAnimationController sacrificeAnimationController;
    public SacrificeStates SacrificeState;
    public SacrificeDataModel SacrificeDataModel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SacrificeState == SacrificeStates.WalkingToExit || SacrificeState == SacrificeStates.WalkingToTree && !sacrificeAnimationController.IsWalking)
        {
            sacrificeAnimationController.SetWalkBool(true);
        }
        else if (SacrificeState == SacrificeStates.FallingDown && !sacrificeAnimationController.IsFalling)
        {
            sacrificeAnimationController.SetWalkBool(false);
            sacrificeAnimationController.SetFallingBool(true);
        }
        else
        {
            sacrificeAnimationController.SetWalkBool(false);
        }
    }

    public void Move(Vector3 targetPosition, float walkingDuration, SacrificeStates stateAfterMovement)
    {
        transform.DOMove(targetPosition, walkingDuration).onComplete = () => SacrificeState = stateAfterMovement;
    }

    public void FallIntoHole()
    {
        SacrificeState = SacrificeStates.FallingDown;
        transform.DOKill();
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
    }

    public void TakeDamage(int damage)
    {
        SacrificeDataModel.Hp -= damage;
        if (SacrificeDataModel.Hp <= 0)
            SacrificeState = SacrificeStates.Dead;
        else if (SacrificeDataModel.Hp < SacrificeDataModel.MaxHp * SacrificeDataModel.PercentageHpLossToStun)
            SacrificeState = SacrificeStates.Stunned;
    }

}
