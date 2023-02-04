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
        [SerializeField] private CultistsGroupController cultistsGroupController;

        public event Action<int> OnCultistsAmountChanged;

        private readonly List<CultistController> _cultists = new();
        private readonly Vector2 _cultistSpawnPositionOffsetMinMax = new(0.1f, 2);

        private void Awake()
        {
            cultistsGroupController.SetCultistGroup(_cultists);
        }

        public void BuyCultist()
        {
            var cultist = Instantiate(cultistPrefab, GetRandomCultistSpawnPosition(), cultistPrefab.transform.rotation,
                transform);
            var cultistController = cultist.GetComponent<CultistController>();
            cultistController.OnReachedSacrifice += CultistController_OnReachedSacrifice;
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

        public void StartMovingCultistGroup(Transform sacrificeTransform)
        {
            cultistsGroupController.MoveCultistsGroupToSacrifice(sacrificeTransform);
        }

        private void StartSacrificeFight()
        {
            //todo: add sacrificeData
            //cultistsGroupController.StartSacrificeFight();
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
            StartSacrificeFight();
        }
    }
}

