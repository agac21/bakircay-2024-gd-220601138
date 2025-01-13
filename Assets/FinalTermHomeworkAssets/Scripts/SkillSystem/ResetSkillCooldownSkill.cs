namespace FinalTermHomeworkAssets.Scripts.SkillSystem
{
    public class ResetSkillCooldownSkill : Skill
    {
        private Skill _skill;

        public ResetSkillCooldownSkill(Skill skill, int duration) : base(duration)
        {
            _skill = skill;
        }

        protected override void ApplyInner()
        {
            if (_skill.IsActive)
            {
                return;
            }

            _skill.TimeTask?.Reset();
            _skill.IsActive = true;
        }

        
    }
}