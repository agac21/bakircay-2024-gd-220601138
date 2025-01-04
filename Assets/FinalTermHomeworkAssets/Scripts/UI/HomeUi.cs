using FinalTermHomeworkAssets.Scripts.Gameplay.SceneObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FinalTermHomeworkAssets.Scripts.UI
{
    public class HomeUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_objectCounter;
        [SerializeField] private TextMeshProUGUI m_scoreText;
        [SerializeField] private Button m_spawnBtn;
        private GameManager _gameManager;

        public void Init(GameManager gameManager)
        {
            _gameManager = gameManager;
            m_spawnBtn.onClick.AddListener(gameManager.ObjectSpawner.GenerateObjects);

            gameManager.EventHub.OnAdd += OnAdd;
            gameManager.EventHub.OnRemove += OnRemove;
            gameManager.EventHub.OnScoreChanged += OnScoreChanged;

            OnScoreChanged(gameManager.ObjectTracker.MatchedScore);
        }

        private void OnScoreChanged(int score)
        {
            m_scoreText.SetText($"Score:{score}");
        }

        public void DeInit()
        {
            if (_gameManager == null)
            {
                return;
            }

            _gameManager.EventHub.OnAdd -= OnAdd;
            _gameManager.EventHub.OnRemove -= OnRemove;
            _gameManager.EventHub.OnScoreChanged -= OnScoreChanged;
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