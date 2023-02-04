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
        [SerializeField] private SacrificeManager sacrificeManager;
        [SerializeField] private DifficultyManager difficultyManager;
        [SerializeField] private LeverManager leverManager;

        
        public UiManager UiManager => uiManager;
        public void Awake()
        {
            SetReferences();
            InitializeManagers();
            moneyManager.OnMoneyAmountChanged += MoneyManager_OnMoneyAmountChanged;
            sacrificeManager.OnSacrificeSpawned += SacrificeManager_OnSacrificeSpawned;
        }
        
        public bool CanAfford(BuyableObjectType objectType)
        {
            return moneyManager.CanAfford(pricesData.Prices.First(x => x.objectType == objectType).price);
        }

        public void BuyFlipper(Transform flipperSpotTransform)
        {
            flippersManager.BuyFlipper(flipperSpotTransform);
            SpendMoney(pricesData.Prices.First(x => x.objectType == BuyableObjectType.FlipperSpot).price);
            flippersManager.SetSacrificeTransformInFlippers(sacrificeManager.GetSacrificeTransform());
        }

        public int GetCurrentDifficulty()
        {
            return difficultyManager.CurrentDifficulty;
        }

        public void UseLever()
        {
            leverManager.UseLever();
        }
        

        private void SetReferences()
        {
            flippersManager.AddFlippers();
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
        
        private void SacrificeManager_OnSacrificeSpawned(Transform sacrifice)
        {
            flippersManager.SetSacrificeTransformInFlippers(sacrifice);
        }

        private void InitializeManagers()
        {
            sacrificeManager.Initialize(this);
        }
    }
}