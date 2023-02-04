namespace Cultist
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using DG.Tweening;
    using UnityEngine;

    public class CultistsGroupController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem fightParticleSystem;
        [SerializeField] private Transform bodyAboveCultistsTransform;
        [SerializeField] private float moveBodyAboveCultistsDuration;

        public event Action OnFightWon;
        public event Action OnCultistLost;
        
        private List<CultistController> _cultists = new();
        private SacrificeController _sacrificeController;

        private const float FightDuration = 5f;

        public void SetReferences(List<CultistController> cultists)
        {
            _cultists = cultists;
        }
    
        public void MoveCultistsGroupToSacrifice()
        {
            foreach (var cultistController in _cultists)
            {
                cultistController.MoveToSacrifice();
            }
        }
        
        public void MoveCultistsToStartingPosition()
        {
            foreach (var cultistController in _cultists)
            {
                cultistController.MoveToAltar();
            }
        }

        public void TransportBody()
        {
            MoveBodyAboveCultists();
            
        }

        public void StartSacrificeFight(SacrificeController sacrificeController)
        {
            _sacrificeController = sacrificeController;
            fightParticleSystem.Play();
            StartCoroutine(Fight());
        }

        private void ConcludeFight()
        {
            switch (_sacrificeController.SacrificeState)
            {
                case SacrificeStates.Stunned:
                    WinFight();
                    break;
                
                case SacrificeStates.FightingCultists:
                    LoseCultist();
                    WinFight();
                    break;
            }
        }

        private void WinFight()
        {
            OnFightWon?.Invoke();
        }

        private void LoseCultist()
        {
            OnCultistLost?.Invoke();
        }

        private void MoveBodyAboveCultists()
        {
            _sacrificeController.transform.DOMove(bodyAboveCultistsTransform.position, moveBodyAboveCultistsDuration).OnComplete(MoveCultistsGroupToAltar);
        }
        
        private void MoveCultistsGroupToAltar()
        {
            foreach (var cultistController in _cultists)
            {
                cultistController.MoveToAltar();
            }
        }

        private IEnumerator Fight()
        {
            yield return new WaitForSeconds(FightDuration);
            ConcludeFight();
        }
    }

}