using FinalTermHomeworkAssets.Scripts.SkillSystem;

namespace FinalTermHomeworkAssets.Scripts
{
    public class SkillHub
    {
        public AutoMatchSkill AutoMatchSkill { get; private set; }
        public ResetSkillCooldownSkill ResetSkillCooldownSkill { get; private set; }

        public SkillHub(ObjectTracker tracker, GameEventHub eventHub)
        {
            AutoMatchSkill = new AutoMatchSkill(tracker, eventHub, 300);
            ResetSkillCooldownSkill = new ResetSkillCooldownSkill(AutoMatchSkill, 300);
        }

        public void ResetSkills()
        {
            AutoMatchSkill.Reset();
            ResetSkillCooldownSkill.Reset();
        }
    }
}