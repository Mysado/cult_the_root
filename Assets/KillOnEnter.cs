using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sacrifice"))
        {
            Destroy(other.gameObject.transform.root.gameObject);
        }
    }
}
