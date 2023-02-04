namespace Gameplay
{
    using System.Linq;
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MoneyManager moneyManager;
        [SerializeField] private FlippersManager flippersManager;
        [SerializeField] private UiManager uiManager;
        [SerializeField] private SacrificeManager sacrificeManager;
        [SerializeField] private DifficultyManager difficultyManager;
        [SerializeField] private LeverManager leverManager;
        [SerializeField] private GameDataHolder gameDataHolder;
        [SerializeField] private TrapsManager trapsManager;
        [SerializeField] private CameraManager cameraManager;

        public GameDataHolder GameDataHolder => gameDataHolder;
        public UiManager UiManager => uiManager;
        public void Awake()
        {
            SetReferences();
            InitializeManagers();
            moneyManager.OnMoneyAmountChanged += MoneyManager_OnMoneyAmountChanged;
            sacrificeManager.OnSacrificeSpawned += SacrificeManager_OnSacrificeSpawned;
            leverManager.OnSacrificeDropped += LeverManager_OnSacrificeDropped;
            trapsManager.OnSacrificeReachedBottom += TrapsManager_OnSacrificeReachedBottom;
        }
        
        public bool CanAfford(BuyableObjectType objectType)
        {
            return moneyManager.CanAfford(gameDataHolder.PricesData.Prices.First(x => x.objectType == objectType).price);
        }

        public bool CanAfford(TrapTypes trapType,int level)
        {
            return moneyManager.CanAfford(gameDataHolder.TrapDatas.First(x => x.TrapType == trapType).UpgradeCostAndDamageForThatLevel[level].UpgradeCost);
        }

        public void BuyFlipper(Transform flipperSpotTransform)
        {
            flippersManager.BuyFlipper(flipperSpotTransform);
            SpendMoney(gameDataHolder.PricesData.Prices.First(x => x.objectType == BuyableObjectType.FlipperSpot).price);
            flippersManager.SetSacrificeTransformInFlippers(sacrificeManager.GetSacrificeTransform());
        }
        
        public void BuyTrap(Transform trapSpotTransform, TrapData trapData)
        {
            trapsManager.BuyTrap(trapSpotTransform, trapData);
            SpendMoney(gameDataHolder.TrapDatas.First(x => x.TrapType == trapData.TrapType).UpgradeCostAndDamageForThatLevel[0].UpgradeCost);
        }
        
        public void UpgradeTrap(TrapController trapController)
        {
            var trapCost = gameDataHolder.TrapDatas.First(x => x.TrapType == trapController.TrapDataModel.TrapType)
                .UpgradeCostAndDamageForThatLevel[trapController.TrapDataModel.CurrentLevel].UpgradeCost;
            SpendMoney(trapCost);
            trapsManager.UpgradeTrap(trapController);
        }

        public int GetCurrentDifficulty()
        {
            return difficultyManager.CurrentDifficulty;
        }

        public void UseLever()
        {
            leverManager.UseLever();
        }

        public TrapData GetTrapData(TrapTypes trapType)
        {
            return gameDataHolder.TrapDatas.First(x => x.TrapType == trapType);
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

        private void LeverManager_OnSacrificeDropped()
        {
            cameraManager.ChangeCameraLocation(CameraLocation.Middle);
        }
        
        private void TrapsManager_OnSacrificeReachedBottom()
        {
            cameraManager.ChangeCameraLocation(CameraLocation.Down);
        }

        private void InitializeManagers()
        {
            sacrificeManager.Initialize(this);
        }
    }
}