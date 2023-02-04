using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class FlippersManager : MonoBehaviour
{

    [SerializeField] private GameObject flipperPrefab;
    private List<Flipper> _flippers = new();

    public void AddFlippers()
    {
        var flippers = GameObject.FindGameObjectsWithTag(StaticManager.TAG_FLIPPER);
        foreach (var flipper in flippers)
        {
            AddFlipper(flipper.GetComponent<Flipper>());
        }
    }
    
    public void SetSacrificeTransformInFlippers(Transform sacrificeTransform)
    {
        foreach (var flipper in _flippers)
        {
            flipper.SetSacrificeTransform(sacrificeTransform);
        }
    }

    public void BuyFlipper(Transform flipperSpotTransform)
    {
        var flipper = Instantiate(flipperPrefab, flipperSpotTransform.position,flipperPrefab.transform.rotation, transform);
        _flippers.Add(flipper.GetComponent<Flipper>());
    }
    
    private void AddFlipper(Flipper flipper)
    {
        _flippers.Add(flipper);
    }

}
