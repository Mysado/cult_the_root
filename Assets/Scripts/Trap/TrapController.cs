using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public TrapDataModel TrapDataModel;
    // Start is called before the first frame update
    void Start()
    {
        
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
