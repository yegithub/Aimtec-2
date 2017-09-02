using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.Yasuo.Core
{
    internal class SpellConfig
    {
        public static Spell Q, W, E, R;

        /// <summary>
        /// Instances the spells
        /// </summary>
        public static void Load()
        {
            Q = new Spell(SpellSlot.Q, 520);
            Q.SetSkillshot(0.25f, 60, 1600, false, SkillshotType.Line);
            Extension.CurrentMode = Mode.Normal;

            W = new Spell(SpellSlot.W, 400);

            E = new Spell(SpellSlot.E, 475);

            R = new Spell(SpellSlot.R, 1400);
        }

        public static void SetSkill(Mode mode)
        {
            switch (mode)
            {
                case Mode.Normal:
                    Q.SetSkillshot(0.25f, 60, 1600, false, SkillshotType.Line);
                    Q.Range = 520;
                    break;
                case Mode.Tornado:
                    Q.SetSkillshot(0.25f, 90, 1200, false, SkillshotType.Line);
                    Q.Range = 1100;
                    break;
                case Mode.Dashing:
                case Mode.DashingTornado:
                    Q.SetSkillshot(0.4333f, 375, 543, false, SkillshotType.Circle);
                    Q.Range = 400;  
                    break;
             
            }
        }
    }
}
