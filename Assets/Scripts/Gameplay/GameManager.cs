namespace Gameplay
{
    using System.Linq;
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MoneyManager moneyManager;
        [SerializeField] private FlippersManager flippersManager;
        [SerializeField] private UiManager uiManager;
        [SerializeField] private PricesData pricesData;

        #region RegionForSacrificeTransformTesting
        
        public Transform testSacrificeTransform;

        #endregion
        
        public void Awake()
        {
            SetReferences();

            moneyManager.OnMoneyAmountChanged += MoneyManager_OnMoneyAmountChanged;
            
            //todo: add trapdoor manager and subscribe this method to event
            OnTrapdoorUsed();
        }
        
        public bool CanAfford(BuyableObjectType objectType)
        {
            return moneyManager.CanAfford(pricesData.Prices.First(x => x.objectType == objectType).price);
        }

        public void BuyFlipper(Transform flipperSpotTransform)
        {
            flippersManager.BuyFlipper(flipperSpotTransform);
            SpendMoney(pricesData.Prices.First(x => x.objectType == BuyableObjectType.FlipperSpot).price);
            //for test
            flippersManager.SetSacrificeTransformInFlippers(testSacrificeTransform);
        }

        private void SetReferences()
        {
            flippersManager.AddFlippers();
        }

        private void OnTrapdoorUsed()
        {
            //todo: get sacrifice transform from sacrifice manager;
            flippersManager.SetSacrificeTransformInFlippers(testSacrificeTransform);
        }

        private void AddMoney(int addAmount)
        {
            moneyManager.AddMoney(addAmount);
        }

        private void SpendMoney(int spendAmount)
        {
            moneyManager.SpendMoney(spendAmount);
        }
        
        private void FlippersManager_OnFlipperBuy(int spendAmount)
        {
            SpendMoney(spendAmount);
        }

        private void MoneyManager_OnMoneyAmountChanged(int moneyAmount)
        {
            uiManager.OnMoneyAmountChangedText(moneyAmount);
        }

    }
}