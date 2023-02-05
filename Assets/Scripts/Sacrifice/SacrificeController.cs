using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
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
    [SerializeField] private Color32 healthyHpColor;
    [SerializeField] private Color32 stunnedHpColor;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private Image hpBackground;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Image sliderBackground;
    [SerializeField] private GameObject canvasParent;
    [SerializeField]
    public SacrificeStates SacrificeState;
    public SacrificeDataModel SacrificeDataModel;
    private Camera mainCamera;

    public void Initialize()
    {
        hpText.text = SacrificeDataModel.Hp + "/" + SacrificeDataModel.MaxHp;
        hpSlider.maxValue = SacrificeDataModel.MaxHp;
        hpSlider.value = SacrificeDataModel.Hp;
        hpSlider.minValue = 0;
        hpBackground.color = healthyHpColor;
        sliderBackground.color = healthyHpColor;
        mainCamera = Camera.main;
    }
    [SerializeField] private AudioSource sacrificeAudioSource;

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

        canvasParent.transform.eulerAngles = new Vector3(0,-90,0);
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
        {
            hpBackground.color = stunnedHpColor;
            sliderBackground.color = stunnedHpColor;
            SacrificeState = SacrificeStates.Stunned;
    }
        hpText.text = SacrificeDataModel.Hp + "/" + SacrificeDataModel.MaxHp;
        hpSlider.value = SacrificeDataModel.Hp;
    }

    public void DisableSacrificeHp()
    {
        canvasParent.SetActive(false);
    }

}
