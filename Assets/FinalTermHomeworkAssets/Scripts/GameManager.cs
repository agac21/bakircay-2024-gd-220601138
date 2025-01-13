using FinalTermHomeworkAssets.Scripts.Gameplay.SceneObjects;
using FinalTermHomeworkAssets.Scripts.InputSystem;
using FinalTermHomeworkAssets.Scripts.TimeSystem;
using FinalTermHomeworkAssets.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;

namespace FinalTermHomeworkAssets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InteractionManager m_interactionManager;
        [SerializeField] private ObjectSpawner m_objectSpawner;
        [SerializeField] private HomeUi m_homeUi;

        private IInputEventHandler _inputEventHandler;

        public ObjectTracker ObjectTracker { get; private set; }

        public ObjectSpawner ObjectSpawner => m_objectSpawner;

        public GameEventHub EventHub { get; private set; }

        public SkillHub SkillHub { get; private set; }

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
            TimeTaskManager.Instance.Initialize();
            _inputEventHandler = GetComponentInChildren<IInputEventHandler>();

            EventHub = new GameEventHub();
            ObjectTracker = new ObjectTracker(EventHub, m_interactionManager.placementZone);
            SkillHub = new SkillHub(ObjectTracker, EventHub);

            m_objectSpawner.SetDependencies(EventHub);

            m_interactionManager.SetDependencies(_inputEventHandler, EventHub);

            ObjectTracker.Init();
            m_homeUi.Init(this);
            m_interactionManager.Initialize();

            m_objectSpawner.GenerateObjects();

            EventHub.OnRemove += OnChanged;
        }

        private void OnChanged(IInteractableObject obj)
        {
            if (ObjectTracker.GetList().Count == 0)
            {
                m_objectSpawner.GenerateObjects();
            }
        }

        private void DeInit()
        {
            ObjectTracker.DeInit();
            m_interactionManager.CleanUp();
            m_homeUi.DeInit();
            EventHub.OnRemove -= OnChanged;
        }

        public void ResetGame()
        {
            for (var i = ObjectTracker.GetList().Count - 1; i >= 0; i--)
            {
                var o = ObjectTracker.GetList()[i];
                Destroy(o.GetTransform().gameObject);
                EventHub.Remove(o);
            }

            SkillHub.ResetSkills();
            ObjectTracker.ResetScore();
            m_objectSpawner.GenerateObjects();
        }
    }
}