using System.Collections.Generic;
using UnityEngine;

public class FlippersManager : MonoBehaviour
{
    private List<Flipper> _flippers;

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
