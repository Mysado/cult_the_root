using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrapController : MonoBehaviour
{
    [SerializeField] private GameObject trapCenter;
    [InfoBox("Lowe - faster")]
    [SerializeField] [Range(0.1f, 1.0f)]private float rotationDuration;
    public TrapDataModel TrapDataModel;

    private bool spinDirection;
    // Start is called before the first frame update
    void Start()
    {
        spinDirection = Random.Range(0, 2) == 0;
        var randomRotation = spinDirection ? Random.Range(40.0f, 55.0f) : Random.Range(-55.0f, -40.0f);
        trapCenter.transform.DOLocalRotate(new Vector3(randomRotation, 0.0f, 0.0f), rotationDuration).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sacrifice"))
        {
            var controller = other.GetComponent<SacrificeController>();
            if(controller && TrapDataModel != null)
                other.GetComponent<SacrificeController>().TakeDamage(TrapDataModel.UpgradeCostAndDamage[TrapDataModel.CurrentLevel].DamageForThatLevel, TrapDataModel.IsLethal);
        }
    }

    public void UpgradeTrap()
    {
        TrapDataModel.CurrentLevel++;
    }

    public void ChangeSpinDirection()
    {
        spinDirection = !spinDirection;
        trapCenter.transform.DOKill();
        trapCenter.transform.rotation = quaternion.identity;
        var randomRotation = spinDirection ? Random.Range(40.0f, 55.0f) : Random.Range(-55.0f, -40.0f);
        trapCenter.transform.DOLocalRotate(new Vector3(randomRotation, 0.0f, 0.0f), rotationDuration).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }
}
