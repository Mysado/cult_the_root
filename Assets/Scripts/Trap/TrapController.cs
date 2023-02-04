using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] private GameObject trapCenter;
    [InfoBox("Lowe - faster")]
    [SerializeField] [Range(0.1f, 1.0f)]private float rotationDuration;
    public TrapDataModel TrapDataModel;
    // Start is called before the first frame update
    void Start()
    {
        trapCenter.transform.DOLocalRotate(new Vector3(50.0f, 0.0f, 0.0f), rotationDuration).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sacrifice"))
        {
            other.GetComponent<SacrificeController>().TakeDamage(TrapDataModel.UpgradeCostAndDamage[TrapDataModel.CurrentLevel].DamageForThatLevel);
        }
    }

    public void UpgradeTrap()
    {
        TrapDataModel.CurrentLevel++;
    }
}
