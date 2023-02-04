using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomHoleController : MonoBehaviour
{
    public event Action OnSacrificeReachedBottom; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sacrifice"))
        {
            OnSacrificeReachedBottom?.Invoke();
        }
    }
}
