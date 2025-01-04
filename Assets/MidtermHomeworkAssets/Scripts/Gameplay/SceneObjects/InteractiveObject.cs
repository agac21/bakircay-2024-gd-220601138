using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace MidtermHomeworkAssets.Scripts.Gameplay.SceneObjects
{
    public class InteractiveObject : MonoBehaviour, IInteractableObject
    {
        private Rigidbody _rigidbody;
        private Vector3 _originalScale;
        private Color _originalColor;

        [SerializeField] private Renderer objectRenderer;
        [SerializeField] private float movementSpeed;

        public bool IsInteractable { get; set; } = true;

        public void OnDeSelect()
        {
            SetDefaultColor();
        }

        public string ObjectId { get; set; }


        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void OnEnter()
        {
            _rigidbody.isKinematic = true;
            var position = transform.position;
            position.y = 2f;
            transform.position = position;

            _originalScale = transform.localScale;
            transform.localScale *= 1.4f;
            transform.eulerAngles = Vector3.zero;

            SetGreenColor();
        }

        private void SetGreenColor()
        {
            var material = objectRenderer.material;
            _originalColor = material.color;
            material.color = Color.green;

            objectRenderer.material = material;
        }

        public void OnMove(Vector2 inputDelta)
        {
            var moveVector = new Vector3(inputDelta.x, 0, inputDelta.y);
            transform.position += moveVector * movementSpeed;
        }

        public void OnExit()
        {
            SetDefaultScale();
            _rigidbody.isKinematic = false;
        }

        private void SetDefaultScale()
        {
            transform.localScale = _originalScale;
        }

        private void SetDefaultColor()
        {
            var material = objectRenderer.material;
            material.color = _originalColor;
            objectRenderer.material = material;
        }

        public void SetOriginalColor(Color color)
        {
            var material = objectRenderer.material;
            material.color = color;
            objectRenderer.material = material;
            _originalColor = color;
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}