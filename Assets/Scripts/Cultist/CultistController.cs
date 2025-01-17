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
        private Transform _altarTransform;

        private const float WalkingDuration = 3.0f;
        private readonly Vector2 cultistMinMaxMoveTimeOffset = new(1.5f, 2.5f);

        private Coroutine movementCoroutine;
        private Animator animator;
    
        public void SetReferences(Vector3 randomCultistPosition, Transform altarTransform)
        {
            _startingPosition = randomCultistPosition;
            _altarTransform = altarTransform;
            animator = GetComponent<Animator>();
        }
        
        public void UpgradeCultist()
        {
        
        }
    
        public void MoveToSacrifice(Transform destination)
        {
            if(movementCoroutine != null)
                StopCoroutine(movementCoroutine);
            transform.DORotate(new Vector3(0,180,0), 0.3f);
            movementCoroutine = StartCoroutine(MoveToSacrifice(destination.position, CultistMoveDestination.Sacrifice, GetRandomTimeOffset()));
        }

        public void MoveToAltar()
        {            
            if(movementCoroutine != null)
                StopCoroutine(movementCoroutine);
            transform.DORotate(new Vector3(0,0,0), 0.3f);
            movementCoroutine = StartCoroutine(Move(_altarTransform.position, CultistMoveDestination.Altar, GetRandomTimeOffset(0.1f)));
        }

        public void MoveToStartPosition()
        {            
            if(movementCoroutine != null)
                StopCoroutine(movementCoroutine);
            transform.DORotate(new Vector3(0,180,0), 0.3f);

            movementCoroutine = StartCoroutine(Move(_startingPosition, CultistMoveDestination.StartingPosition, GetRandomTimeOffset(0.1f)));
        }

        private float GetRandomTimeOffset(float multiplier = 1.0f)
        {
            return Random.Range(cultistMinMaxMoveTimeOffset.x * multiplier, cultistMinMaxMoveTimeOffset.y * multiplier);
        }

        private void DestinationReached(CultistMoveDestination destinationType)
        {
            animator.SetBool("IsWalking", false);
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
            animator.SetBool("IsWalking", true);
            yield return new WaitForSeconds(timeOffset);
            transform.DOMove(destination, WalkingDuration).OnComplete(() => DestinationReached(destinationType));
        }
        
        private IEnumerator MoveToSacrifice(Vector3 destination, CultistMoveDestination destinationType,  float timeOffset)
        {
            animator.SetBool("IsWalking", true);
            yield return new WaitForSeconds(timeOffset);
            destination.y = _altarTransform.position.y;
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
