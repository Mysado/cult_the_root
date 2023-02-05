using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
    [SerializeField]
    public SacrificeStates SacrificeState;
    public SacrificeDataModel SacrificeDataModel;
    [SerializeField] private AudioSource sacrificeAudioSource;

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
            
        sacrificeAudioSource.PlayOneShot(SacrificeDataModel.ScreamsOfPain[Random.Range(0, SacrificeDataModel.ScreamsOfPain.Count)]);
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
