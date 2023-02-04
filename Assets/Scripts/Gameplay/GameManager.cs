namespace Gameplay
{
    using System.Linq;
    using Cultist;
    using DefaultNamespace.Altar;
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
        [SerializeField] private CultistsManager cultistsManager;
        [SerializeField] private TrapsManager trapsManager;
        [SerializeField] private CameraManager cameraManager;
        [SerializeField] private AltarManager altarManager;

        public GameDataHolder GameDataHolder => gameDataHolder;
        
        public UiManager UiManager => uiManager;
        
        public void Awake()
        {
            SetReferences();
            moneyManager.OnMoneyAmountChanged += MoneyManager_OnMoneyAmountChanged;
            sacrificeManager.OnSacrificeSpawned += SacrificeManager_OnSacrificeSpawned;
            cultistsManager.OnCultistsAmountChanged += CultistsManager_OnCultistsAmountChanged;
            cultistsManager.OnReachedAltar += CultistsManager_OnReachedAltar;

            uiManager.OnBuyCultist += UiManager_OnBuyCultist;
            uiManager.OnUpgradeCultists += UiManager_OnUpgradeCultists;
            leverManager.OnSacrificeDropped += LeverManager_OnSacrificeDropped;
            trapsManager.OnSacrificeReachedBottom += TrapsManager_OnSacrificeReachedBottom;
            altarManager.OnGainExperience += AltarManager_OnGainExperience;
            InitializeManagers();
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

        public SacrificeController GetCurrentSacrifice()
        {
            return sacrificeManager.GetSacrificeController();
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

        private void CultistsManager_OnReachedAltar()
        {
            Debug.Log("Hello, you have been sacrificed");
            altarManager.ReceiveBody(GetCurrentSacrifice());
        }

        private void UiManager_OnBuyCultist()
        {
            cultistsManager.SpawnCultist();
        }

        private void UiManager_OnUpgradeCultists()
        {
            cultistsManager.UpgradeCultists();
        }

        private void LeverManager_OnSacrificeDropped()
        {
            cameraManager.ChangeCameraLocation(CameraLocation.Middle);
        }
        
        private void TrapsManager_OnSacrificeReachedBottom()
        {
            cameraManager.ChangeCameraLocation(CameraLocation.Down);
            cultistsManager.StartMovingCultistGroup();
        }

        private void AltarManager_OnGainExperience(int exp)
        {
            AddMoney(exp);
        }

        private void InitializeManagers()
        {
            sacrificeManager.Initialize(this);
            moneyManager.Initialize();
        }
    }
}