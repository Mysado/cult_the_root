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
        [SerializeField] private Transform bodyAboveAltarTransform;
        [SerializeField] private float moveBodyAboveCultistsDuration;

        public event Action OnFightWon;
        public event Action OnCultistLost;
        
        private List<CultistController> _cultists = new();
        private SacrificeController _sacrificeController;

        private const float FightDuration = 1f;
        private const float MovingToAltarDuration = 1.0f;

        public void SetReferences(List<CultistController> cultists)
        {
            _cultists = cultists;
        }
    
        public void MoveCultistsGroupToSacrifice(SacrificeController sacrificeController)
        {
            _sacrificeController = sacrificeController;
            foreach (var cultistController in _cultists)
            {
                cultistController.MoveToSacrifice();
            }
        }
        
        public void MoveCultistsToStartingPosition()
        {
            foreach (var cultistController in _cultists)
            {
                cultistController.MoveToStartPosition();
            }
        }

        public void TransportBody()
        {
            DisableGravityForBody();
            MoveBodyAboveCultists();
            
        }

        public void StartSacrificeFight()
        {
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

        private void DisableGravityForBody()
        {
            Debug.Log("Disabling Grav");
            _sacrificeController.GetComponent<Rigidbody>().isKinematic = true;
            _sacrificeController.GetComponent<Rigidbody>().useGravity = false;
        }

        private void MoveBodyAboveCultists()
        {
            Debug.Log("move body above cultists");
            _sacrificeController.transform.DOMove(bodyAboveCultistsTransform.position, moveBodyAboveCultistsDuration)
                                .OnComplete(MoveCultistsGroupToAltar);
        }

        private void MoveBodyToAltar()
        {
            _sacrificeController.transform.DOMove(bodyAboveAltarTransform.position, MovingToAltarDuration);
        }
        
        private void MoveCultistsGroupToAltar()
        {
            Debug.Log("Move Cultists To Altar");
            Debug.Log("Cultists amount: " + _cultists.Count);
            foreach (var cultistController in _cultists)
            {
                cultistController.MoveToAltar();
            }
            MoveBodyToAltar();
        }

        private IEnumerator Fight()
        {
            yield return new WaitForSeconds(FightDuration);
            ConcludeFight();
        }
    }

}