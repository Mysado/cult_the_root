using System;

namespace Cultist
{
    using System.Collections;
    using System.Collections.Generic;
    using DG.Tweening;
    using UnityEngine;

    public class CultistsGroupController : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> fightParticleSystems;
        [SerializeField] private Transform bodyAboveAltarTransform;
        [SerializeField] private GameObject particleParent;
        [SerializeField] private float moveBodyAboveCultistsDuration;

        public event Action OnFightWon;
        public event Action OnCultistLost;
        
        private List<CultistController> _cultists = new();
        private SacrificeController _sacrificeController;

        private const float FightDuration = 1f;
        private const float MovingToAltarDuration = 3.0f;

        public void SetReferences(List<CultistController> cultists)
        {
            _cultists = cultists;
        }
    
        public void MoveCultistsGroupToSacrifice(SacrificeController sacrificeController)
        {
            _sacrificeController = sacrificeController;
            foreach (var cultistController in _cultists)
            {
                cultistController.MoveToSacrifice(_sacrificeController.transform);
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
            particleParent.transform.position = _cultists[Random.Range(0, _cultists.Count - 1)].transform.position + Vector3.up + (Vector3.right * 2);
            fightParticleSystems.ForEach(x => x.Play());
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
                   
                    var amountOfLostCultists =Mathf.CeilToInt(_sacrificeController.SacrificeDataModel.Hp -
                    _sacrificeController.SacrificeDataModel.PercentageHpLossToStun *
                        _sacrificeController.SacrificeDataModel.MaxHp);
                    for (int i = 0; i < amountOfLostCultists; i++)
                    {
                        LoseCultist();
                    }
                    WinFight();
                    break;
                default:
                    Debug.LogError("Sacrifice neither healthy nor stunned nor dead");
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
            _sacrificeController.GetComponent<Rigidbody>().isKinematic = true;
            _sacrificeController.GetComponent<Rigidbody>().useGravity = false;
        }

        private void MoveBodyAboveCultists()
        {
            var position = _cultists[Random.Range(0, _cultists.Count - 1)].transform.position + Vector3.up;
            _sacrificeController.transform.DOMove(position, moveBodyAboveCultistsDuration)
                                .OnComplete(MoveCultistsGroupToAltar);
        }

        private void MoveBodyToAltar()
        {
            _sacrificeController.transform.DOMove(bodyAboveAltarTransform.position, MovingToAltarDuration);
        }
        
        private void MoveCultistsGroupToAltar()
        {
            foreach (var cultistController in _cultists)
            {
                cultistController.MoveToAltar();
            }
            Invoke(nameof(MoveBodyToAltar),0.2f);
        }

        private IEnumerator Fight()
        {
            yield return new WaitForSeconds(FightDuration);
            ConcludeFight();
        }
    }

}