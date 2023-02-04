namespace Gameplay
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    [CreateAssetMenu(menuName = "ScriptableObjects/Prices")]
    public class PricesData : ScriptableObject
    {
        [SerializeField] private List<BuyableObjectData> prices;

        public List<BuyableObjectData> Prices => prices;
    }

    [Serializable]
    public class BuyableObjectData
    {
        public BuyableObjectType objectType;
        public int price;
    }

    public enum BuyableObjectType
    {
        FlipperSpot,
        TrapSpot,
        Cultist,
        CultistsUpgrade
    }
}