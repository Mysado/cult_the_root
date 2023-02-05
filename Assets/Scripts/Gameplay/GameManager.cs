using System;
using DG.Tweening;

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
        [SerializeField] private SkinnedMeshRenderer treeRenderer;
        [SerializeField] private SoundManager soundManager;

        public GameDataHolder GameDataHolder => gameDataHolder;
        
        public UiManager UiManager => uiManager;

        private float treeBlendShapeValue;
        
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
            trapsManager.OnTrapHit += TrapsManager_OnTrapHit;
            altarManager.OnGainExperience += AltarManager_OnGainExperience;
            cameraManager.OnCameraReachedSurfaceAfterSacrifice += CameraManager_OnCameraReachedSurfaceAfterSacrifice;
            altarManager.OnAltarEmptied += AltarManager_OnAltarEmptied;
            InitializeManagers();
            DOTween.To(() => treeBlendShapeValue, x => treeBlendShapeValue = x, 100, 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

            PlayMusic();
        }

        private void Update()
        {
            treeRenderer.SetBlendShapeWeight(0, treeBlendShapeValue);
        }

        public bool CanAfford(BuyableObjectType objectType)
        {
            return moneyManager.CanAfford(gameDataHolder.PricesData.Prices.First(x => x.objectType == objectType).price);
        }

        public bool CanAfford(TrapTypes trapType,int level)
        {
            PlaySfx(SfxType.SlotClick);
            return moneyManager.CanAfford(gameDataHolder.TrapDatas.First(x => x.TrapType == trapType).UpgradeCostAndDamageForThatLevel[level].UpgradeCost);
        }

        public void BuyFlipper(Transform flipperSpotTransform)
        {
            flippersManager.BuyFlipper(flipperSpotTransform);
            SpendMoney(gameDataHolder.PricesData.Prices.First(x => x.objectType == BuyableObjectType.FlipperSpot).price);
            flippersManager.SetSacrificeTransformInFlippers(sacrificeManager.GetSacrificeTransform());
            PlaySfx(SfxType.BuyTrap);
        }
        
        public void BuyTrap(Transform trapSpotTransform, TrapData trapData)
        {
            trapsManager.BuyTrap(trapSpotTransform, trapData);
            SpendMoney(gameDataHolder.TrapDatas.First(x => x.TrapType == trapData.TrapType).UpgradeCostAndDamageForThatLevel[0].UpgradeCost);
            PlaySfx(SfxType.BuyTrap);
        }
        
        public void UpgradeTrap(TrapController trapController)
        {
            var trapCost = gameDataHolder.TrapDatas.First(x => x.TrapType == trapController.TrapDataModel.TrapType)
                .UpgradeCostAndDamageForThatLevel[trapController.TrapDataModel.CurrentLevel].UpgradeCost;
            SpendMoney(trapCost);
            trapsManager.UpgradeTrap(trapController);
            PlaySfx(SfxType.BuyTrap);
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

        public void ChangeTrapSpinDirection(TrapController trapController)
        {
            trapsManager.ChangeSpinDirection(trapController);
        }

        public void PlayMusic()
        {
            soundManager.PlayMusic();
        }

        public void PlaySfx(SfxType sfxType)
        {
            soundManager.PlaySfx(sfxType);
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
            PlaySfx(SfxType.BuyCultist);
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
            PlaySfx(SfxType.BodyHitFloor);
            cameraManager.ChangeCameraLocation(CameraLocation.Down);
            cultistsManager.StartMovingCultistGroup();
        }
        
        private void TrapsManager_OnTrapHit(TrapTypes trapType)
        {
            switch (trapType)
            {
                case TrapTypes.Boots:
                    soundManager.PlaySfx(SfxType.BootTrapDmg);
                    break;
                case TrapTypes.Swords:
                    soundManager.PlaySfx(SfxType.SwordTrapDmg);
                    break;
                case TrapTypes.Maces:
                    soundManager.PlaySfx(SfxType.MaceTrapDmg);
                    break;
            }
        }

        private void AltarManager_OnGainExperience(int exp)
        {
            AddMoney(exp);
            sacrificeManager.SacrificeSacrificed();
        }

        private void CameraManager_OnCameraReachedSurfaceAfterSacrifice()
        {
            sacrificeManager.SacrificeSacrificed();
        }
        
        private void AltarManager_OnAltarEmptied()
        {
            soundManager.PlaySfx(SfxType.LifeDrain);
            difficultyManager.IncreaseDifficulty();
            cameraManager.MoveCameraToSurface();
        }

        private void InitializeManagers()
        {
            sacrificeManager.Initialize(this);
            moneyManager.Initialize();
            cultistsManager.Initialize();
        }
    }
}