using UnityEngine;

namespace FinalTermHomeworkAssets.Scripts.Gameplay.SceneObjects
{
    public interface IInteractableObject
    {
        public void OnEnter();
        public void OnExit();
        public void OnDeSelect();
        public string ObjectId { get;  set; }

        public Transform GetTransform();

        public bool IsInteractable { get; set; }
    }
}