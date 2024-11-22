using MidtermHomeworkAssets.Scripts.Gameplay.SceneObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MidtermHomeworkAssets.Scripts.UI
{
    public class HomeUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_objectCounter;
        [SerializeField] private Button m_spawnBtn;
        private GameManager _gameManager;

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;
            m_spawnBtn.onClick.AddListener(gameManager.ObjectSpawner.GenerateObjects);

            gameManager.EventHub.OnAdd += OnAdd;
            gameManager.EventHub.OnRemove += OnRemove;
        }

        public void DeInit()
        {
            if (_gameManager == null)
            {
                return;
            }

            _gameManager.EventHub.OnAdd -= OnAdd;
            _gameManager.EventHub.OnRemove -= OnRemove;
        }


        private void OnAdd(IInteractableObject obj)
        {
            SetCounterText();
            SetSpawnButtonActive();
        }

        private void SetCounterText()
        {
            m_objectCounter.SetText($"Remained objects:{_gameManager.ObjectTracker.GetList().Count}");
        }

        private void OnRemove(IInteractableObject obj)
        {
            SetCounterText();
            SetSpawnButtonActive();
        }

        private void SetSpawnButtonActive()
        {
            m_spawnBtn.gameObject.SetActive(_gameManager.ObjectTracker.GetList().Count == 0);
        }
    }
}