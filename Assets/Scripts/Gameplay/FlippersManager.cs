using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class FlippersManager : MonoBehaviour
{
    public Flipper testFlipper;
    
    public event Action<int> OnFlipperBuy; 

    private List<Flipper> _flippers = new();
    
    private const int FlipperCost = 45;

    public void AddFlippers()
    {
        var flippers = GameObject.FindGameObjectsWithTag(Tags.FLIPPER);
        foreach (var flipper in flippers)
        {
            AddFlipper(flipper.GetComponent<Flipper>());
        }
    }
    
    private void AddFlipper(Flipper flipper)
    {
        _flippers.Add(flipper);
        flipper.OnFlipperBuy += BuyFlipper;
    }

    public void SetSacrificeTransformInFlippers(Transform sacrificeTransform)
    {
        foreach (var flipper in _flippers)
        {
            flipper.SetSacrificeTransform(sacrificeTransform);
        }
    }

    private void BuyFlipper()
    {
        OnFlipperBuy?.Invoke(FlipperCost);
    }
}
