using System;
using UnityEngine;

namespace MidtermHomeworkAssets.Scripts.InputSystem
{
    public interface IInputEventHandler
    {
        public event Action OnInputDown;
        public event Action<Vector2> OnInputDrag;
        public event Action OnInputUp;

        public Vector2 GetInputPosition();
    }
}