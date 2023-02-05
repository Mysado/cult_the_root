namespace Gameplay
{
    using DefaultNamespace;
    using UnityEngine;

    public class RaycastManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private LayerMask layerMask;

        private Transform trapBuyTransform;
        private void Update()
        {
            var hitObject = CastRay();
            if (hitObject)
            {
                Hover(hitObject);
                if (Input.GetMouseButtonDown(0))
                {
                    Interact(hitObject);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    TryToChangeTrapSpinDirection(hitObject);
                }
            }
            else
            {
                gameManager.UiManager.CloseTooltip();
            }
        }

        private GameObject CastRay()
        {
            var ray = mainCamera.ScreenPointToRay (Input.mousePosition);
            var layer = LayerMask.NameToLayer("Interactable");
            if (Physics.Raycast(ray, out var hit, 100, 1 << layer))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer(StaticManager.LAYER_INTERACTABLE))
                {
                    return hit.transform.gameObject;
                }
            }

            return null;
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
                        gameManager.BuyFlipper(raycastHit.transform);
                        raycastHit.SetActive(false);
                    }
                    break;
                case StaticManager.TAG_LEVER:
                    gameManager.UseLever();
                    break;
                case StaticManager.TAG_TRAP_SPOT:
                    trapBuyTransform = raycastHit.transform;
                    gameManager.UiManager.ShowTrapSelection(trapBuyTransform.position);
                    break;
                case StaticManager.TAG_TRAP:
                    var trap = raycastHit.GetComponent<TrapController>();
                    if (gameManager.CanAfford(trap.TrapDataModel.TrapType,0))
                    {
                        gameManager.UpgradeTrap(trap);
                    }
                    break;
            }
        }
        
        private void Hover(GameObject raycastHit)
        {
            switch (raycastHit.tag)
            {
                case StaticManager.TAG_FLIPPER_SPOT:
                    gameManager.UiManager.ShowTooltip(raycastHit.transform.position, "Buy Flipper for threefiddy");
                    break;
                case StaticManager.TAG_TRAP_SPOT:
                    gameManager.UiManager.ShowTooltip(raycastHit.transform.position, "Buy Trap for threefiddy");
                    break;
                case StaticManager.TAG_TRAP:
                    var trap = raycastHit.GetComponent<TrapController>();
                    gameManager.UiManager.ShowTooltip(raycastHit.transform.position, "Upgrade "+trap.TrapDataModel.TrapType+" to " + (trap.TrapDataModel.CurrentLevel+1) +" level for threefiddy");
                    break;
            }
        }

        private void TryToChangeTrapSpinDirection(GameObject raycastHit)
        {
            if(raycastHit.CompareTag(StaticManager.TAG_TRAP))
            {
                var trap = raycastHit.GetComponent<TrapController>();
                gameManager.ChangeTrapSpinDirection(trap);
            }
        }

        public void SelectTrapToBuy(int trapType)
        {
            if (gameManager.CanAfford((TrapTypes)trapType,0))
            {
                gameManager.BuyTrap(trapBuyTransform,gameManager.GetTrapData((TrapTypes)trapType));
                trapBuyTransform.gameObject.SetActive(false);
                gameManager.UiManager.CloseTrapSelection();
            }
        }

    }
}