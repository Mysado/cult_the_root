namespace Cultist
{
    using System;
    using System.Collections;
    using DG.Tweening;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class CultistController : MonoBehaviour
    {
        public event Action OnReachedSacrifice;
        public event Action OnReachedAltar;
        public event Action OnReachedStartingPosition;

        private CultistDataModel _cultistDataModel;
        private Vector3 _startingPosition;
        private Transform _sacrificeTransform;
        private Transform _altarTransform;

        private const float WalkingDuration = 1.0f;
        private readonly Vector2 cultistMinMaxMoveTimeOffset = new(0.1f, 0.3f);

        private Coroutine movementCoroutine;
    
        public void SetReferences(Vector3 randomCultistPosition, Transform altarTransform, Transform sacrificeTransform)
        {
            _startingPosition = randomCultistPosition;
            _altarTransform = altarTransform;
            _sacrificeTransform = sacrificeTransform;
        }
        
        public void UpgradeCultist()
        {
        
        }
    
        public void MoveToSacrifice()
        {
            if(movementCoroutine != null)
                StopCoroutine(movementCoroutine);
            movementCoroutine = StartCoroutine(Move(_sacrificeTransform.position, CultistMoveDestination.Sacrifice, GetRandomTimeOffset()));
        }

        public void MoveToAltar()
        {            
            if(movementCoroutine != null)
                StopCoroutine(movementCoroutine);
            movementCoroutine = StartCoroutine(Move(_altarTransform.position, CultistMoveDestination.Altar, GetRandomTimeOffset()));
        }

        public void MoveToStartPosition()
        {            
            if(movementCoroutine != null)
                StopCoroutine(movementCoroutine);
            movementCoroutine = StartCoroutine(Move(_startingPosition, CultistMoveDestination.StartingPosition, GetRandomTimeOffset()));
        }

        private float GetRandomTimeOffset()
        {
            return Random.Range(cultistMinMaxMoveTimeOffset.x, cultistMinMaxMoveTimeOffset.y);
        }

        private void DestinationReached(CultistMoveDestination destinationType)
        {
            switch (destinationType)
            {
                case CultistMoveDestination.Sacrifice:
                    OnReachedSacrifice?.Invoke();
                    break;
                case CultistMoveDestination.Altar:
                    OnReachedAltar?.Invoke();
                    break;
                case CultistMoveDestination.StartingPosition:
                    OnReachedStartingPosition?.Invoke();
                    break;
            }
        }

        private IEnumerator Move(Vector3 destination, CultistMoveDestination destinationType,  float timeOffset)
        {
            yield return new WaitForSeconds(timeOffset);
            transform.DOMove(destination, WalkingDuration).OnComplete(() => DestinationReached(destinationType));
        }
    }

    public enum CultistMoveDestination
    {
        Sacrifice,
        Altar,
        StartingPosition
    }
}
