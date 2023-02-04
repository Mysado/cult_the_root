using DG.Tweening;
using Gameplay;
using UnityEngine;

public class Flipper : MonoBehaviour, InteractableObject
{
    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] private float minimalDistanceToSacrifice;
    
    private bool _rotationInProgress;
    private int _currentRotationIndex;
    private FlipperRotationValue[] _flipperRotationValues =
        { new(-45, 0, 0), new(45, 0, 0) };

    private Transform _sacrificeTransform;

    private void Awake()
    {
        FlipRotation();
    }

    public void SetSacrificeTransform(Transform sacrificeTransform)
    {
        _sacrificeTransform = sacrificeTransform;
    }
    
    public void Interact()
    {
        if (!CanChangeState()) { return; }
    
        _rotationInProgress = true;
        SetRotationIndex();
        FlipRotation();
    }

    private bool CanChangeState()
    {
        if (_rotationInProgress) { return false; }
        
        var distanceToSacrifice = CalculateDistanceToSacrifice();
        if (distanceToSacrifice < minimalDistanceToSacrifice)
        {
            return false;
        }

        return true;
    }

    private float CalculateDistanceToSacrifice()
    {
        return Vector3.Distance(transform.position, _sacrificeTransform.position);
    } 

    private void SetRotationIndex()
    {
        _currentRotationIndex++;
        if (_currentRotationIndex >= _flipperRotationValues.Length)
        {
            _currentRotationIndex = 0;
        }
    }

    private void FlipRotation()
    {
        transform.DORotate(_flipperRotationValues[_currentRotationIndex].FlipperRotation,rotationDuration).OnComplete(() => _rotationInProgress = false);
    }
}

public class FlipperRotationValue
{
    public Vector3 FlipperRotation { get; }

    public FlipperRotationValue(int x, int y, int z)
    {
        FlipperRotation = new Vector3(x,y,z);
    }
}
