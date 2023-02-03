using System.Collections.Generic;
using UnityEngine;

public class FlippersManager : MonoBehaviour
{
    #region RegionForTesting
    
    public Flipper testFlipper;
    public Transform testSacrificeTransform;
    
    private void Awake()
    {
        AddFlipper(testFlipper);
        SetSacrificeTransformInFlippers(testSacrificeTransform);
    }
    
    #endregion
    
    private List<Flipper> _flippers = new List<Flipper>();

    public void AddFlipper(Flipper flipper)
    {
        _flippers.Add(flipper);
    }

    public void SetSacrificeTransformInFlippers(Transform sacrificeTransform)
    {
        foreach (var flipper in _flippers)
        {
            flipper.SetSacrificeTransform(sacrificeTransform);
        }
    }
}
