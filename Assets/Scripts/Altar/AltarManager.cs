﻿namespace DefaultNamespace.Altar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DG.Tweening;
    using Gameplay;
    using Sirenix.Serialization;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class AltarManager : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem lifeForceAbsorptionParticles;
        [SerializeField] 
        private List<CorpseSlot> corpseSlots = new();
        [SerializeField] 
        private float bodyToCorpseSlotMoveDuration;
        [SerializeField] private GameManager gameManager;

        public event Action<int> OnGainExperience; 

        private SacrificeController _sacrificeController;

        public void ReceiveBody(SacrificeController sacrificeController)
        {
            _sacrificeController = sacrificeController;
            StartLifeForceAbsorptionParticles();
            MoveBodyToRandomCorpseSlot();
        }

        private void StartLifeForceAbsorptionParticles()
        {
            lifeForceAbsorptionParticles.Play(); 
        }

        private void MoveBodyToRandomCorpseSlot()
        {
            var randomCorpseSlot = GetRandomNotOccupiedCorpseSlot();
            _sacrificeController.transform.DOMove(randomCorpseSlot.transform.position, bodyToCorpseSlotMoveDuration).OnComplete(() => GainExpFromCorpse(_sacrificeController.SacrificeDataModel.ExpWorth, _sacrificeController.SacrificeState));
            randomCorpseSlot.Occupied = true;
        }

        private CorpseSlot GetRandomNotOccupiedCorpseSlot()
        {
            var notOccupiedCorpseSlots = corpseSlots.Where(x => x.Occupied == false).ToList();
            return notOccupiedCorpseSlots[Random.Range(0, notOccupiedCorpseSlots.Count)];
        }

        private void GainExpFromCorpse(int expWorth, SacrificeStates state)
        {
            if (state == SacrificeStates.Dead)
            {
                OnGainExperience?.Invoke(expWorth/2);
            } 
            else
            {
                OnGainExperience?.Invoke(expWorth);
            }
        }
    }
}