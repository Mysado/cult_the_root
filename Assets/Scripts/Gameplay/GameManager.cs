namespace Gameplay
{
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MoneyManager moneyManager;
        [SerializeField] private FlippersManager flippersManager;
        [SerializeField] private UiManager uiManager;

        #region RegionForSacrificeTransformTesting
        
        public Transform testSacrificeTransform;

        #endregion

        private void Awake()
        {
            SetReferences();
            
            flippersManager.OnFlipperBuy += FlippersManager_OnFlipperBuy;
            
            moneyManager.OnMoneyAmountChanged += MoneyManager_OnMoneyAmountChanged;
            
            //todo: add trapdoor manager and subscribe this method to event
            OnTrapdoorUsed();
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