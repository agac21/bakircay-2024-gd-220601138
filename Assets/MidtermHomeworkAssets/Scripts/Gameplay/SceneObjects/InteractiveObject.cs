using System.Collections;
using UnityEngine;

namespace MidtermHomeworkAssets.Scripts.Gameplay.SceneObjects
{
    public class InteractiveObject : MonoBehaviour, IInteractableObject
    {
        private Rigidbody _rigidbody;
        private Vector3 _originalScale;
        private Color _originalColor;

        [SerializeField] private Renderer objectRenderer;
        [SerializeField] private AnimationCurve scaleAnimationCurve;
        [SerializeField] private float movementSpeed;

        public bool IsInteractable { get; private set; } = true;


        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _originalColor = Random.ColorHSV();

            objectRenderer.material.color = _originalColor;
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

            var material = objectRenderer.material;
            _originalColor = material.color;
            material.color = Color.green;

            objectRenderer.material = material;
        }

        public void OnMove(Vector2 inputDelta)
        {
            var moveVector = new Vector3(inputDelta.x, 0, inputDelta.y);
            transform.position += moveVector * movementSpeed * Time.deltaTime;
        }

        public void OnExit()
        {
            transform.localScale = _originalScale;
            _rigidbody.isKinematic = false;

            var material = objectRenderer.material;
            material.color = _originalColor;
            objectRenderer.material = material;
        }

        public void OnPutPlacementArea(PlacementArea area)
        {
            StartCoroutine(ScaleDownCoroutine());
        }

        private IEnumerator ScaleDownCoroutine()
        {
            IsInteractable = false;
            const float duration = 0.15f;
            var timeElapsed = 0f;
            var initialScale = transform.localScale;

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                var scaleFactor = scaleAnimationCurve.Evaluate(Mathf.Min(1, timeElapsed / duration));
                transform.localScale = initialScale * scaleFactor;
                yield return null;
            }

            IsInteractable = true;
            Destroy(gameObject);
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}