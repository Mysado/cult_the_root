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
        private int _cultistsReachedSacrifice;
        private int _cultistsReachedAltar;
        private int _cultistsReachedStartingPos;

        private readonly List<CultistController> _cultists = new();
        private readonly Vector2 _cultistSpawnPositionOffsetMinMax = new(0.1f, 2);

        public void Initialize()
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
            cultistController.OnReachedStartingPosition += CultistController_OnReachedStartingPosition;
            cultistController.SetReferences(randomCultistPosition, altarTransform, sacrificeTransform);
            AddCultistToGroup(cultistController);
            OnCultistsAmountChanged?.Invoke(_cultists.Count);
        }

        public void LoseCultist()
        {
            var cultistToRemove = _cultists[Random.Range(0, _cultists.Count)];
            cultistToRemove.GetComponent<Animator>().SetTrigger("Dead");
            _cultists.Remove(cultistToRemove);
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
            if(_currentSacrifice.SacrificeState is not (SacrificeStates.Dead or SacrificeStates.Stunned or SacrificeStates.FightingCultists or SacrificeStates.FallingDown))
                return;
            if (_currentSacrifice.SacrificeState is not (SacrificeStates.Stunned or SacrificeStates.Dead))
            {
                _currentSacrifice.SacrificeState = SacrificeStates.FightingCultists;
            }
            cultistsGroupController.MoveCultistsGroupToSacrifice(_currentSacrifice);
        }

        private void TransportBody()
        {
            cultistsGroupController.TransportBody();
        }

        private void StartSacrificeFight()
        {
            cultistsGroupController.StartSacrificeFight();
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

            if (_cultistsReachedSacrifice < _cultists.Count-1)
            {
                _cultistsReachedSacrifice++;
                return;
            }
            _cultistsReachedSacrifice = 0;
            if (_currentSacrifice.SacrificeState == SacrificeStates.Dead || _currentSacrifice.SacrificeState == SacrificeStates.Stunned)
            {
                TransportBody();
                return;
            }
            StartSacrificeFight();
        }

        private void CultistController_OnReachedAltar()
        {
            if (_cultistsReachedAltar < _cultists.Count-1)
            {
                _cultistsReachedAltar++;
                return;
            }
            _cultistsReachedAltar = 0;
            OnReachedAltar?.Invoke();
            cultistsGroupController.MoveCultistsToStartingPosition();
            _cultistsReachedAltar = 0;
        }

        private void CultistController_OnReachedStartingPosition()
        {
            if (_cultistsReachedStartingPos < _cultists.Count-1)
            {
                _cultistsReachedStartingPos++;
                return;
            }
            //do something here
            _cultistsReachedStartingPos = 0;
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

