namespace Cultist
{
    using System;
    using System.Collections.Generic;
    using Gameplay;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class CultistsManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject cultistPrefab;
        [SerializeField] private Transform cultistsSpawnPosition;
        [SerializeField] private Transform altarTransform;
        [SerializeField] private Transform sacrificeTransform;
        [SerializeField] private CultistsGroupController cultistsGroupController;

        public event Action<int> OnCultistsAmountChanged;
        public event Action OnReachedAltar;

        private SacrificeController _currentSacrifice;

        private readonly List<CultistController> _cultists = new();
        private readonly Vector2 _cultistSpawnPositionOffsetMinMax = new(0.1f, 2);

        private void Awake()
        {
            cultistsGroupController.SetReferences(_cultists);
            cultistsGroupController.OnFightWon += CultistGroupController_OnFightWon;
            cultistsGroupController.OnCultistLost += CultistGroupController_OnCultistLost;
            SpawnCultist();
            SpawnCultist();
            SpawnCultist();
            SpawnCultist();
        }

        public void SpawnCultist()
        {
            var randomCultistPosition = GetRandomCultistSpawnPosition();
            var cultist = Instantiate(cultistPrefab, randomCultistPosition, cultistPrefab.transform.rotation,
                transform);
            var cultistController = cultist.GetComponent<CultistController>();
            cultistController.OnReachedSacrifice += CultistController_OnReachedSacrifice;
            cultistController.OnReachedAltar += CultistController_OnReachedAltar;
            cultistController.SetReferences(randomCultistPosition, altarTransform, sacrificeTransform);
            AddCultistToGroup(cultistController);
            OnCultistsAmountChanged?.Invoke(_cultists.Count);
        }

        public void LoseCultist()
        {
            _cultists.RemoveAt(Random.Range(0, _cultists.Count));
            OnCultistsAmountChanged?.Invoke(_cultists.Count);
            //add cultist die sound and maybe some animation?
        }

        public void UpgradeCultists()
        {
            foreach (var cultistController in _cultists)
            {
                cultistController.UpgradeCultist();
            }
        }

        public void StartMovingCultistGroup()
        {
            _currentSacrifice = gameManager.GetCurrentSacrifice();
            cultistsGroupController.MoveCultistsGroupToSacrifice();
        }

        private void TransportBody()
        {
            cultistsGroupController.TransportBody();
        }

        private void StartSacrificeFight(SacrificeController sacrificeController)
        {
            cultistsGroupController.StartSacrificeFight(sacrificeController);
        }

        private void AddCultistToGroup(CultistController cultistController)
        {
            _cultists.Add(cultistController);
        }

        private Vector3 GetRandomCultistSpawnPosition()
        {
            var offsetX = Random.Range(_cultistSpawnPositionOffsetMinMax.x, _cultistSpawnPositionOffsetMinMax.y);
            var offsetZ = Random.Range(_cultistSpawnPositionOffsetMinMax.x, _cultistSpawnPositionOffsetMinMax.y);
            var spawnPos = cultistsSpawnPosition.position;
            return new Vector3(spawnPos.x + offsetX, spawnPos.y,
                spawnPos.z + offsetZ);
        }

        private void CultistController_OnReachedSacrifice()
        {
            if (_currentSacrifice.SacrificeState == SacrificeStates.Dead)
            {
                TransportBody();
                return;
            }
            StartSacrificeFight(_currentSacrifice);
        }

        private void CultistController_OnReachedAltar()
        {
            OnReachedAltar?.Invoke();
            cultistsGroupController.MoveCultistsToStartingPosition();
        }

        private void CultistGroupController_OnCultistLost()
        {
            LoseCultist();
        }

        private void CultistGroupController_OnFightWon()
        {
            TransportBody();
        }
    }
}

