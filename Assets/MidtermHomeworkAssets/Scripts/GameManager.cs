using System;
using MidtermHomeworkAssets.Scripts.Gameplay.SceneObjects;
using MidtermHomeworkAssets.Scripts.InputSystem;
using MidtermHomeworkAssets.Scripts.UI;
using UnityEngine;

namespace MidtermHomeworkAssets.Scripts
{
    public class GameEventHub
    {
        public event Action<IInteractableObject> OnAdd;
        public event Action<IInteractableObject> OnRemove;

        public void Add(IInteractableObject obj)
        {
            OnAdd?.Invoke(obj);
        }

        public void Remove(IInteractableObject obj)
        {
            OnRemove?.Invoke(obj);
        }
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InteractionManager m_interactionManager;
        [SerializeField] private ObjectSpawner m_objectSpawner;
        [SerializeField] private HomeUi m_homeUi;

        private IInputEventHandler _inputEventHandler;

        public ObjectTracker ObjectTracker { get; private set; }

        public ObjectSpawner ObjectSpawner => m_objectSpawner;

        public GameEventHub EventHub { get; private set; }

        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            DeInit();
        }

        private void Init()
        {
            _inputEventHandler = GetComponentInChildren<IInputEventHandler>();
            EventHub = new GameEventHub();
            ObjectTracker = new ObjectTracker(EventHub);

            m_objectSpawner.SetDependencies(EventHub);

            m_interactionManager.SetDependencies(_inputEventHandler, EventHub);
            
            ObjectTracker.Init();
            m_homeUi.Init(this);
            m_interactionManager.Initialize();

            m_objectSpawner.GenerateObjects();
        }

        private void DeInit()
        {
            ObjectTracker.DeInit();
            m_interactionManager.CleanUp();
            m_homeUi.DeInit();
        }
    }
}