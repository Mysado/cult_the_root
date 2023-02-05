using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

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
    [FormerlySerializedAs("rigidbody")] [SerializeField] private Rigidbody rb;
    [SerializeField] private SacrificeAnimationController sacrificeAnimationController;
    [SerializeField] private int hp;
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

        hp = SacrificeDataModel.Hp;
    }

    public void Move(Vector3 targetPosition, float walkingDuration, SacrificeStates stateAfterMovement)
    {
        transform.DOMove(targetPosition, walkingDuration).onComplete = () => SacrificeState = stateAfterMovement;
    }

    public void FallIntoHole()
    {
        SacrificeState = SacrificeStates.FallingDown;
        transform.DOKill();
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void TakeDamage(int damage, bool isLethal)
    {
        SacrificeDataModel.Hp -= damage;
        if (SacrificeDataModel.Hp <= 0)
        {
            if (!isLethal)
                SacrificeDataModel.Hp = 1;
            else
                SacrificeState = SacrificeStates.Dead;
        }        
        else if (SacrificeDataModel.Hp < SacrificeDataModel.MaxHp * SacrificeDataModel.PercentageHpLossToStun)
            SacrificeState = SacrificeStates.Stunned;
    }

}
