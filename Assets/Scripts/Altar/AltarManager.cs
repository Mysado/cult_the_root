namespace DefaultNamespace.Altar
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

        public event Action<int> OnGainExperience; 
        public event Action OnAltarEmptied; 

        private SacrificeController _sacrificeController;

        public void ReceiveBody(SacrificeController sacrificeController)
        {
            _sacrificeController = sacrificeController;
            MoveBodyToRandomCorpseSlot();
        }

        private void StartLifeForceAbsorption()
        {
            lifeForceAbsorptionParticles.Play();
            foreach (var corpseSlot in corpseSlots)
            {
                corpseSlot.BodyInSlot.transform.DOScale(Vector3.zero, 1).OnComplete(() => Destroy(corpseSlot.BodyInSlot));
                corpseSlot.Occupied = false;
                corpseSlot.BodyInSlot = null;
            }
            OnAltarEmptied?.Invoke();
        }

        private void MoveBodyToRandomCorpseSlot()
        {
            var randomCorpseSlot = GetRandomNotOccupiedCorpseSlot();
            _sacrificeController.DisableSacrificeHp();
            _sacrificeController.transform.DOMove(randomCorpseSlot.transform.position, bodyToCorpseSlotMoveDuration).OnComplete(() => GainExpFromCorpse(_sacrificeController.SacrificeDataModel.ExpWorth, _sacrificeController.SacrificeState));
            randomCorpseSlot.Occupied = true;
            randomCorpseSlot.BodyInSlot = _sacrificeController.gameObject;
            if (corpseSlots.Where(x => x.Occupied == false).ToList().Count == 0)
            {
                StartLifeForceAbsorption();
            }
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
                OnGainExperience?.Invoke(expWorth/10);
            } 
            else
            {
                OnGainExperience?.Invoke(expWorth);
            }
        }
    }
}