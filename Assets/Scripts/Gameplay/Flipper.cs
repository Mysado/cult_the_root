using UnityEngine;

public class Flipper : MonoBehaviour
{
    private int _currentRotationIndex;
    private FlipperRotationValue[] _flipperRotationValues =
        { new(0, 0, -45), new(0, 0, 45) };

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            SetRotationIndex();
            FlipRotation();
        }
    }

    private void SetRotationIndex()
    {
        _currentRotationIndex++;
        if (_currentRotationIndex > _flipperRotationValues.Length)
        {
            _currentRotationIndex = 0;
        }
    }

    private void FlipRotation()
    {
        //Add DoTween
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
