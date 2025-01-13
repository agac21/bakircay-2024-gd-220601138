using FinalTermHomeworkAssets.Scripts.TimeSystem.UI;
using FinalTermHomeworkAssets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace FinalTermHomeworkAssets.Scripts.SkillSystem.UI
{
    public class SkillButton : MonoBehaviour
    {
        [SerializeField] private GameObject m_completeView;
        [SerializeField] private Button m_btn;
        [SerializeField] private TimeTaskView m_timer;
        private Skill _skill;

        public void Init(Skill skill)
        {
            _skill = skill;

            _skill.OnActiveToggled += OnActiveToggledChanged;
            m_btn.onClick.RemoveAllListeners();
            m_btn.onClick.AddListener(() => skill.TryApply());

            m_completeView.gameObject.SetActive(true);
            m_timer.gameObject.SetActive(false);
        }

        private void OnActiveToggledChanged(bool obj)
        {
            m_completeView.gameObject.SetActive(obj);
            m_timer.gameObject.SetActive(!obj);
            if (!obj) m_timer.Init(_skill.TimeTask);
        }

        public void DeInit()
        {
            m_timer.DeInit();
        }
    }
}