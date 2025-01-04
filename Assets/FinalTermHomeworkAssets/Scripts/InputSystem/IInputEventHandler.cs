using System;
using UnityEngine;

namespace FinalTermHomeworkAssets.Scripts.InputSystem
{
    public interface IInputEventHandler
    {
        public event Action OnInputDown;
        public event Action OnInputDrag;
        public event Action OnInputUp;

        public Vector2 GetInputPosition();
    }
}