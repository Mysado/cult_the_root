namespace Gameplay
{
    using DefaultNamespace;
    using UnityEngine;

    public class RaycastManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameManager gameManager;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CastRay();
            }
        }

        private void CastRay()
        {
            var ray = mainCamera.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer(StaticManager.LAYER_INTERACTABLE))
                {
                    Interact(hit.transform.gameObject);
                }
            }
        }

        private void Interact(GameObject raycastHit)
        {
            switch (raycastHit.tag)
            {
                case StaticManager.TAG_FLIPPER:
                    raycastHit.GetComponent<Flipper>().Interact();
                    break;
                
                case StaticManager.TAG_FLIPPER_SPOT:
                    if (gameManager.CanAfford(BuyableObjectType.FlipperSpot))
                    {
                        gameManager.BuyFlipper(raycastHit.gameObject.transform);
                        raycastHit.gameObject.SetActive(false);
                    }
                    break;

            }
        }
    }
}