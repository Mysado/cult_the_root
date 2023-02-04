namespace Gameplay
{
    using System.Linq;
    using Cultist;
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
        [SerializeField] private CultistsManager cultistsManager;
        
        public UiManager UiManager => uiManager;
        
        public void Awake()
        {
            SetReferences();
            InitializeManagers();
            moneyManager.OnMoneyAmountChanged += MoneyManager_OnMoneyAmountChanged;
            sacrificeManager.OnSacrificeSpawned += SacrificeManager_OnSacrificeSpawned;
            cultistsManager.OnCultistsAmountChanged += CultistsManager_OnCultistsAmountChanged;

            uiManager.OnBuyCultist += UiManager_OnBuyCultist;
            uiManager.OnUpgradeCultists += UiManager_OnUpgradeCultists;
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

        private void CultistsManager_OnCultistsAmountChanged(int cultistsAmount)
        {
            uiManager.OnCultistsAmountChanged(cultistsAmount);
        }

        private void UiManager_OnBuyCultist()
        {
            cultistsManager.BuyCultist();
        }

        private void UiManager_OnUpgradeCultists()
        {
            cultistsManager.UpgradeCultists();
        }

        private void InitializeManagers()
        {
            sacrificeManager.Initialize(this);
        }
    }
}