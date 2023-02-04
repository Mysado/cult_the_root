using DG.Tweening;
using UnityEngine;

public class CultistControler : MonoBehaviour
{
    
    public void MoveToSacrifice(Transform sacrificePosition, float walkingDuration)
    {
        transform.DOMove(sacrificePosition.position, walkingDuration);
    }
}
