using MidtermHomeworkAssets.Scripts.Gameplay.SceneObjects;
using MidtermHomeworkAssets.Scripts.InputSystem;
using UnityEngine;

namespace MidtermHomeworkAssets.Scripts
{
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private PlacementArea placementZone;
        private IInputEventHandler _inputHandler;
        private int interactableLayer;

        private IInteractableObject currentInteractable;
        private GameEventHub _eventHub;


        public void SetDependencies(IInputEventHandler eventHandler, GameEventHub eventHub)
        {
            _eventHub = eventHub;
            _inputHandler = eventHandler;
        }

        public void Initialize()
        {
            interactableLayer = LayerMask.GetMask("Interactable");

            _inputHandler.OnInputDown += OnDown;
            _inputHandler.OnInputUp += OnUp;
            _inputHandler.OnInputDrag += OnDrag;
        }

        public void CleanUp()
        {
            _inputHandler.OnInputDown -= OnDown;
            _inputHandler.OnInputUp -= OnUp;
            _inputHandler.OnInputDrag -= OnDrag;
        }

        private void OnDown()
        {
            if (!PerformRaycast(interactableLayer, out var hitInfo)) return;

            var targetInteractable = hitInfo.collider.GetComponentInParent<IInteractableObject>();
            if (targetInteractable is not { IsInteractable: true }) return;

            currentInteractable = targetInteractable;
            currentInteractable.OnEnter();
        }

        private void OnDrag(Vector2 movementDelta)
        {
            currentInteractable?.OnMove(movementDelta);
        }

        private bool PerformRaycast(LayerMask layer, out RaycastHit hitInfo)
        {
            var mainCam = Camera.main;
            var touchRay = mainCam.ScreenPointToRay(_inputHandler.GetInputPosition());
            return Physics.Raycast(touchRay, out hitInfo, 500f, layer);
        }

        private void OnUp()
        {
            if (currentInteractable == null) return;
            if (!currentInteractable.IsInteractable) return;
            if (placementZone.Contains(currentInteractable.GetTransform()))
            {
                _eventHub.Remove(currentInteractable);
                currentInteractable.OnPutPlacementArea(placementZone);
            }
            else currentInteractable.OnExit();

            currentInteractable = null;
        }
    }
}