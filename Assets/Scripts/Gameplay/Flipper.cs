using DG.Tweening;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] private float minimalDistanceToSacrifice;
    
    private bool _rotationInProgress;
    private int _currentRotationIndex;
    private FlipperRotationValue[] _flipperRotationValues =
        { new(0, 0, -45), new(0, 0, 45) };

    private Transform _sacrificeTransform;

    public void SetSacrificeTransform(Transform sacrificeTransform)
    {
        _sacrificeTransform = sacrificeTransform;
    }
    
    private void OnMouseOver()
    {
        if (!Input.GetMouseButton(0)) return;
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
