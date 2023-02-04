namespace Cultist
{
    using System;
    using DG.Tweening;
    using UnityEngine;

    public class CultistController : MonoBehaviour
    {
        public event Action OnReachedSacrifice;

        private CultistDataModel _cultistDataModel;
        private Transform _sacrificeTransform;

        private const float WalkingDuration = 1.0f;
    
        public void UpgradeCultist()
        {
        
        }
    
        public void MoveToSacrifice(Transform sacrificeTransform, float delay)
        {
            _sacrificeTransform = sacrificeTransform;
            Invoke(nameof(MoveToSacrificeWithDelay), delay);
        }

        private void MoveToSacrificeWithDelay()
        {
            transform.DOMove(_sacrificeTransform.position, WalkingDuration).OnComplete(() => OnReachedSacrifice?.Invoke());
        }
    }
}
