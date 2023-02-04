using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    [SerializeField] private Transform trapDoor;
    [SerializeField] private Transform trapDoorCenter;
    [SerializeField] private Transform leverHandle;
    [SerializeField] private Vector3 openRotation;
    [SerializeField] private float openDuration;
    [SerializeField] private float closeDuration;
    
    public event Action OnSacrificeDropped; 

    private bool isInTransition;
    // Start is called before the first frame update

    [Button]
    public void UseLever()
    {
        if(isInTransition)
            return;
        var myFloat = 0;
        trapDoor.DOLocalRotate(openRotation, openDuration).onComplete = CloseTrapdoor;
        leverHandle.DOLocalRotate(new Vector3(0, 0, -30), 0.5f);
        DOTween.To(()=> myFloat, x=> myFloat = x, 52, openDuration/10).onComplete = CheckIfSacrificeIsOnTrapDoor;
    }

    private void CheckIfSacrificeIsOnTrapDoor()
    {
        var colliders = Physics.OverlapBox(trapDoorCenter.position, Vector3.one);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Sacrifice"))
            {
                collider.GetComponent<SacrificeController>().FallIntoHole();
                OnSacrificeDropped?.Invoke();
                return;
            }
        }
    }

    private void CloseTrapdoor()
    {
        trapDoor.DOLocalRotate(Vector3.zero, closeDuration).onComplete = () => isInTransition = false;
        leverHandle.DOLocalRotate(new Vector3(0, 0, 30), 0.5f);
    }
}
