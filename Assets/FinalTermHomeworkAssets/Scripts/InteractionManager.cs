using System;
using DG.Tweening;
using FinalTermHomeworkAssets.Scripts.Gameplay.SceneObjects;
using FinalTermHomeworkAssets.Scripts.InputSystem;
using UnityEngine;

namespace FinalTermHomeworkAssets.Scripts
{
    
    public class InteractionManager : MonoBehaviour
    {
        public PlacementArea placementZone;
        private IInputEventHandler _inputHandler;
        private int interactableLayer;

        private IInteractableObject currentInteractable;
        private GameEventHub _eventHub;
        private Vector3 _offset;
        private float _zCoord;


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
            if (placementZone.GetAddedObject() == currentInteractable) placementZone.Remove(currentInteractable);
            currentInteractable.OnEnter();

            var interactableTransform = currentInteractable.GetTransform();
            var interactableTransformPosition = interactableTransform.position;
            _zCoord = Camera.main.WorldToScreenPoint(interactableTransformPosition).z;
            _offset = interactableTransformPosition - GetMouseWorldPosition();
        }

        private Vector3 GetMouseWorldPosition()
        {
            var mousePoint = Input.mousePosition;
            mousePoint.z = _zCoord;
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }

        private void OnDrag()
        {
            if (currentInteractable == null)
            {
                return;
            }

            var currentPos = currentInteractable.GetTransform().position;
            var targetPos = GetMouseWorldPosition() + _offset;
            targetPos.y = currentPos.y;
            currentInteractable.GetTransform().position = targetPos;
        }

        private bool PerformRaycast(LayerMask layer, out RaycastHit hitInfo)
        {
            var mainCam = Camera.main;
            var touchRay = mainCam.ScreenPointToRay(_inputHandler.GetInputPosition());
            return Physics.Raycast(touchRay, out hitInfo, 500f, layer);
        }

        private void OnUp()
        {
            var current = currentInteractable;
            if (current == null) return;
            if (!current.IsInteractable) return;
            if (placementZone.Contains(current.GetTransform()))
            {
                var otherObject = placementZone.GetAddedObject();
                if (otherObject == null)
                {
                    current.IsInteractable = false;
                    placementZone.OnAdded(current).OnComplete(() => current.IsInteractable = true);
                }
                else
                {
                    if (otherObject.ObjectId == current.ObjectId)
                    {
                        otherObject.IsInteractable = false;
                        current.IsInteractable = false;

                        var seq = DOTween.Sequence();
                        seq.Append(placementZone.OnAdded(current));

                        _eventHub.Remove(otherObject);
                        _eventHub.Remove(current);
                        seq.Append(placementZone.OnMatched());
                        _eventHub.Matched();
                    }
                    else
                    {
                        placementZone.OnThrowAway(current);
                        current.GetTransform().localScale = Vector3.one;
                        current.IsInteractable = true;
                    }
                }
            }
            else current.OnExit();

            current.OnDeSelect();
            currentInteractable = null;
        }
    }
}