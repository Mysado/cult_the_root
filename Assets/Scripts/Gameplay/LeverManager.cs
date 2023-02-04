using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    [SerializeField] private Transform trapDoor;
    [SerializeField] private Transform trapDoorCenter;
    [SerializeField] private Vector3 openRotation;
    [SerializeField] private float openDuration;
    [SerializeField] private float closeDuration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Button]
    public void UseLever()
    {
        var myFloat = 0;
        trapDoor.DORotate(openRotation, openDuration).onComplete = () => trapDoor.DORotate(Vector3.zero, closeDuration);
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
                return;
            }
        }
    }
}
