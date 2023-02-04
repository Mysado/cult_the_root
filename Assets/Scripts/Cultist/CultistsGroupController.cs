namespace Cultist
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class CultistsGroupController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem fightParticleSystem;
        
        private List<CultistController> _cultists = new();
        private SacrificeController _sacrificeController;
        private readonly Vector2 cultistMinMaxMoveTimeOffset = new(0.1f, 0.3f);
    
    
        private const float FightDuration = 5f;

        public void SetCultistGroup(List<CultistController> cultists)
        {
            _cultists = cultists;
        }
    
        public void MoveCultistsGroupToSacrifice(Transform sacrificeTransform)
        {
            foreach (var cultistController in _cultists)
            {
                var randomMoveTimeOffset = Random.Range(cultistMinMaxMoveTimeOffset.x, cultistMinMaxMoveTimeOffset.y);
                cultistController.MoveToSacrifice(sacrificeTransform,randomMoveTimeOffset);
            }
        }

        public void TransportBody(SacrificeController sacrificeController)
        {
            
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
            
        }

        private void LoseCultist()
        {
            
        }

        private IEnumerator Fight()
        {
            yield return new WaitForSeconds(FightDuration);
            ConcludeFight();
        }
    }

}