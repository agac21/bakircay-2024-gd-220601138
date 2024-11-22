using System;
using UnityEngine;

namespace MidtermHomeworkAssets.Scripts.InputSystem
{
    public class MouseEventHandler : MonoBehaviour, IInputEventHandler
    {
        public event Action OnInputDown;
        public event Action<Vector2> OnInputDrag;
        public event Action OnInputUp;


        private bool _isDown;

        private Vector2 _inputPos;
        private Vector2 _lastPos;

        public Vector2 GetInputPosition()
        {
            return Input.mousePosition;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !_isDown)
            {
                OnInputDown?.Invoke();
                _isDown = true;
                _inputPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0) && _isDown)
            {
                var pos = GetInputPosition();
                OnInputDrag?.Invoke(pos - _inputPos);
                _inputPos = pos;
            }

            if (Input.GetMouseButtonUp(0) && _isDown)
            {
                OnInputUp?.Invoke();
                _isDown = false;
            }
        }
    }
}