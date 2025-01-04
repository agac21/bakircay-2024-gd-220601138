using DG.Tweening;
using UnityEngine;

namespace MidtermHomeworkAssets.Scripts.Gameplay.SceneObjects
{
    public interface IInteractableObject
    {
        public void OnEnter();
        public void OnMove(Vector2 inputDelta);
        public void OnExit();
        public void OnDeSelect();
        public string ObjectId { get;  set; }

        public Transform GetTransform();

        public bool IsInteractable { get; set; }
    }
}